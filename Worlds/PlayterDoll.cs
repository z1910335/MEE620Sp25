//============================================================================
// PlayterDoll.cs
// Code for runing the Godot scene of rotating playter doll
//============================================================================
using Godot;
using System;

public partial class PlayterDoll : Node3D
{
	// parameters
	double ma = 0.107;     // arm mass ratio
	double rho = 2.21;     // dimensionless radius of gyration
	double gammay = 0.091; // ratio I_Gy/I_Gx
	double gammaz = 1.05;  // ratio I_Gz/I_Gx
	double h = 1.56;       // dimensionless height of shoulder above CG
	double L = 1.65;       // dimensionless length to arm mass
	double Lrod = 2.5;    // length of arm rod (for visual)
	double phi = 0.0;      // shoulder angle

	float cgHeight;

	// simulation
	PlayterSim sim;
	double time;

	// model
	PDollModel model;

	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	double theta;   // somersault angle for testing

	Label armModeLabel;
	LineEdit[] icOmega;
	SpinBox[] sbIcOmega;
	Button simButton;
	Button resetButton;

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("PlayterDoll");

		cgHeight = 6.0f;
		sim = new PlayterSim();
		time = 0.0;
		SetParamsDoll();

		// set up doll model
		model = GetNode<PDollModel>("PDollModel");
		model.Position = new Vector3(0.0f, cgHeight, 0.0f);
		model.Initialize((float)h, (float)L, (float)Lrod, 3.0f /*hUb*/, 
		3.75f /*hLb*/, 0.5f /*thk*/, (float)phi);

		// Set up the camera rig
		longitudeDeg = 30.0f;  //30
		latitudeDeg = 15.0f;
		camDist = 12.0f;

		camTg = new Vector3(0.0f, cgHeight, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.Target = camTg;
		cam.Position = new Vector3(0.0f, cgHeight, 0.0f);

		theta = 0.0;


		SetupUI();

		/*
		LinSysEq sys;
		int ii,jj;
        Random rnd = new Random();
        sys = new LinSysEq(8);
        for(ii=0; ii<8; ++ii){
            for(jj=0; jj<8; ++jj){
                sys.SetA(ii, jj, rnd.NextDouble());
            }
			sys.SetB(ii, rnd.NextDouble());
        }

		sys.SolveGauss();
		GD.Print("Check = " + sys.Check());
		/**/
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
		//model.SetSimpleRotation((float)theta);
		float q0 = (float)sim.Q0;
		float q1 = (float)sim.Q1;
		float q2 = (float)sim.Q2;
		float q3 = (float)sim.Q3;
		float thL = (float)sim.ThetaL;
		float thR = (float)sim.ThetaR;
		float xG = (float)sim.XG;
		float yG = (float)sim.YG;
		float zG = (float)sim.ZG;

		model.Update(q0, q1, q2, q3, thL, thR, xG, yG, zG);
	}

	//------------------------------------------------------------------------
	// SetParamsDoll
	//------------------------------------------------------------------------
	private void SetParamsDoll()
	{
		ma = 0.107;     // arm mass ratio
		rho = 2.21;     // dimensionless radius of gyration
		gammay = 0.091; // ratio I_Gy/I_Gx
		gammaz = 1.05;  // ratio I_Gz/I_Gx
		h = 1.56;       // dimensionless height of shoulder above CG
		L = 1.65;       // dimensionless length to arm mass
		Lrod = 2.5;    // length of arm rod (for visual)
		phi = 0.0;      // shoulder angle

		sim.ArmMass = ma;
		sim.RadiusOfGyration = rho;
		sim.GammaY = gammay;
		sim.GammaZ = gammaz;
		sim.ShoulderHeight = h;
		sim.ArmLength = L;
		sim.Phi = phi;
	}

	//------------------------------------------------------------------------
	// SetParamsHuman
	//------------------------------------------------------------------------
	private void SetParamsHuman()
	{
		ma = 0.107;     // arm mass ratio
		rho = 2.21;     // dimensionless radius of gyration
		gammay = 0.091; // ratio I_Gy/I_Gx
		gammaz = 1.05;  // ratio I_Gz/I_Gx
		h = 1.56;       // dimensionless height of shoulder above CG
		L = 1.65;       // dimensionless length to arm mass
		Lrod = 2.5;    // length of arm rod (for visual)
		phi = 0.0;      // shoulder angle

		sim.ArmMass = ma;
		sim.RadiusOfGyration = rho;
		sim.GammaY = gammay;
		sim.GammaZ = gammaz;
		sim.ShoulderHeight = h;
		sim.ArmLength = L;
		sim.Phi = phi;
	}

	//------------------------------------------------------------------------
	// _PhysicsProcess:
	//------------------------------------------------------------------------
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		sim.Step(time, delta);
		time += delta;
    }

	//------------------------------------------------------------------------
	// SetupUI
	//------------------------------------------------------------------------
	private void SetupUI()
	{
		int i;

		MarginContainer mcTL = GetNode<MarginContainer>("Control/MgCTL/pContain/MC");

		VBoxContainer vbox = new VBoxContainer();
		mcTL.AddChild(vbox);

		Label titleLabel = new Label();
		titleLabel.Text = "Playter Doll Simulator";
		vbox.AddChild(titleLabel);
		vbox.AddChild(new HSeparator());

		GridContainer grid = new GridContainer();
		grid.Columns = 2;
		vbox.AddChild(grid);

		Label armLabel = new Label();
		armLabel.Text = "Arm Mode:";
		armModeLabel = new Label();
		armModeLabel.Text = "Free";
		grid.AddChild(armLabel);
		grid.AddChild(armModeLabel);
		grid.AddChild(new HSeparator());
		grid.AddChild(new HSeparator());

		Label[] icLabel;
		icLabel = new Label[3];
		icOmega = new LineEdit[3];
		sbIcOmega = new SpinBox[3];
		for(i=0; i<3; ++i){
			icLabel[i] = new Label();
			//icOmega[i] = new LineEdit();
			//icOmega[i].TooltipText = "Enter initial condition.";
			sbIcOmega[i] = new SpinBox();
			grid.AddChild(icLabel[i]);
			//grid.AddChild(icOmega[i]);
			grid.AddChild(sbIcOmega[i]);
			//icOmega[i] = sbIcOmega[i].GetLineEdit();
			sbIcOmega[i].Step = 0.01f;
			sbIcOmega[i].MaxValue = 2.0f;
			sbIcOmega[i].ValueChanged += OnICValueChanged;
			//sbIcOmega[i].Suffix = "rad/s";
		}
		icLabel[0].Text = "IC: OmegaX: ";
		icLabel[1].Text = "IC: OmegaY: ";
		icLabel[2].Text = "IC: OmegaZ: ";

		sbIcOmega[0].Value = 1.0f;
		sbIcOmega[1].Value = 0.0f;
		sbIcOmega[2].Value = 0.0f;

		// icOmega[0].Text = "1.0";
		// icOmega[1].Text = "0.0";
		// icOmega[2].Text = "0.0";

		grid.AddChild(new HSeparator());
		grid.AddChild(new HSeparator());

		simButton = new Button();
		simButton.Text = "Simulate";
		vbox.AddChild(simButton);

		resetButton = new Button();
		resetButton.Text = "Reset";
		vbox.AddChild(resetButton);
	}


	private void OnICValueChanged(double val)
	{
		double omX = sbIcOmega[0].Value;
		double omY = sbIcOmega[1].Value;
		double omZ = sbIcOmega[2].Value;
		
		GD.Print("Omega = " + omX + ", " + omY + ", " + omZ);
	}
}
