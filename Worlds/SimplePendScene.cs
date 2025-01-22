//============================================================================
// SimplePendScene.cs
// Code for runing the Godot scene of a simple pendulum scene
//============================================================================
using Godot;
using System;

public partial class SimplePendScene : Node3D
{
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	StickBall pModel;    // 3D model of pendulum
	double pendLength;   // pendulum length

	enum OpMode
	{
		Config,
		Sim
	}

	OpMode opMode;         // operation mode
	Vector3 pendRotation;  // rotation value for pendulum
	float dthetaMan;   // amount angle is changed each time updated manually
	bool angleManChanged;  // Angle has been changed manually

	SimplePendSim sim;     // object for the simulation
	double time;           // simulation time

	//UIPanelDisplay datDisplay;
	GridIO gridIO;
	int uiRefreshCtr;     //counter for display refresh
	int uiRefreshTHold;   // threshold for display refresh

	Label instructLabel;   // label to display instructions
	String instManual;    // instructions when in manual mode
	String instSim;        // instructions when in sim model
	
	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("Hello MEE 620");

		// Set up the camera rig
		longitudeDeg = 30.0f;
		latitudeDeg = 15.0f;
		camDist = 2.7f;

		camTg = new Vector3(0.0f, 1.0f, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.Target = camTg;

		// Set up simulation
		opMode = OpMode.Config;
		pendLength = 1.0;
		pendRotation = new Vector3();
		dthetaMan = 0.03f;
		angleManChanged = true;
		sim = new SimplePendSim();
		sim.Length = pendLength;
		sim.Angle = 0.0;
		sim.GenSpeed = 0.0;
		time = 0.0;

		// Set up model
		float mountHeight = 1.4f;
		Node3D mnt = GetNode<Node3D>("Axle");
		mnt.Position = new Vector3(0.0f, mountHeight, 0.0f);
		pModel = GetNode<StickBall>("StickBall");
		pModel.Position = new Vector3(0.0f, mountHeight, 0.0f);
		pModel.Rotation = pendRotation;
		pModel.Length = (float)pendLength;
		//pModel.StickDiameter = 0.15f;
		//pModel.BallDiameter = 0.6f;

		// Display
		SetupUI();
		uiRefreshTHold = 3;
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
		if(opMode == OpMode.Config){  // change angle manually
			if(Input.IsActionPressed("ui_right")){
				pendRotation.Z += dthetaMan;
				gridIO.SetNumeric(0,1, Mathf.RadToDeg(pendRotation.Z));
				gridIO.SetText(1,1, "---");
				gridIO.SetText(2,1, "---");
				gridIO.SetText(3,1, "---");
				// datDisplay.SetValue(2, "---");
				// datDisplay.SetValue(3, "---");
				// datDisplay.SetValue(4, "---");
				pModel.Rotation = pendRotation;
				angleManChanged = true;
			}
			if(Input.IsActionPressed("ui_left")){
				pendRotation.Z -= dthetaMan;
				gridIO.SetNumeric(0,1, Mathf.RadToDeg(pendRotation.Z));
				gridIO.SetText(1,1, "---");
				gridIO.SetText(2,1, "---");
				gridIO.SetText(3,1, "---");
				// datDisplay.SetValue(1, Mathf.RadToDeg(pendRotation.Z));
				// datDisplay.SetValue(2, "---");
				// datDisplay.SetValue(3, "---");
				// datDisplay.SetValue(4, "---");
				pModel.Rotation = pendRotation;
				angleManChanged = true;
			}

			if(Input.IsActionJustPressed("ui_accept")){
				if(angleManChanged){
					sim.Angle = (double)pendRotation.Z;
					sim.GenSpeed = 0.0;
				}

				opMode = OpMode.Sim;
				gridIO.SetText(4,1, opMode.ToString());
				instructLabel.Text = instSim;
				angleManChanged = false;
			}
			return;
		}

		// angle determined by simulation
		pendRotation.Z = (float)sim.Angle;
		pModel.Rotation = pendRotation;

		// data display
		if(uiRefreshCtr > uiRefreshTHold){
			float ke = (float)sim.KineticEnergy;
			float pe = (float)sim.PotentialEnergy;

			gridIO.SetNumeric(0,1, Mathf.RadToDeg(pendRotation.Z));
			gridIO.SetNumeric(1,1, ke);
			gridIO.SetNumeric(2,1, pe);
			gridIO.SetNumeric(3,1, ke+pe);

			// datDisplay.SetValue(1, Mathf.RadToDeg(pendRotation.Z));
			// datDisplay.SetValue(2, ke);
			// datDisplay.SetValue(3, pe);
			// datDisplay.SetValue(4, ke+pe);
			uiRefreshCtr = 0;   // reset the counter
		}
		++uiRefreshCtr;

		// Change to manual mode
		if(Input.IsActionJustPressed("ui_accept")){
			opMode = OpMode.Config;

			gridIO.SetText(4,1, opMode.ToString());
			gridIO.SetText(1,1, "---");
			gridIO.SetText(2,1, "---");
			gridIO.SetText(3,1, "---");


			//datDisplay.SetValue(0, opMode.ToString());
			instructLabel.Text = instManual;
			///////// datDisplay.SetValue(2, "---");
			///////// datDisplay.SetValue(3, "---");
			///////// datDisplay.SetValue(4, "---");
		}

	} // end _Process

	//------------------------------------------------------------------------
    // _PhysicsProcess:
    //------------------------------------------------------------------------
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		if(opMode == OpMode.Config)
			return;

		sim.Step(time, delta);
		time += delta;
    }

	//------------------------------------------------------------------------
    // SetupUI():
    //------------------------------------------------------------------------
	private void SetupUI()
	{
		MarginContainer mcBL = GetNode<MarginContainer>(
			"UINode/MargContBL");

		VBoxContainer vBoxBL = new VBoxContainer();
		mcBL.AddChild(vBoxBL);

		Label title = new Label();
		title.Text = "Simple Pendulum";
		vBoxBL.AddChild(title);

		vBoxBL.AddChild(new HSeparator());

		gridIO = new GridIO();
		vBoxBL.AddChild(gridIO);
		gridIO.SetSize(5,2);
		gridIO.InitGridCells();
		gridIO.SetDigitsAfterDecimal(0, 1, 1);
		gridIO.SetDigitsAfterDecimal(1, 1, 4);
		gridIO.SetDigitsAfterDecimal(2, 1, 4);
		gridIO.SetDigitsAfterDecimal(3, 1, 4);
		gridIO.SetText(1,1, "---");
		gridIO.SetText(2,1, "---");
		gridIO.SetText(3,1, "---");
		gridIO.SetText(0,0, "Angle: ");
		gridIO.SetText(1,0, "Kinetic: ");
		gridIO.SetText(2,0, "Potential: ");
		gridIO.SetText(3,0, "Total Erg: ");
		gridIO.SetText(4,0, "Mode:");
		gridIO.SetText(4,1, "Config");

		MarginContainer mcBR = GetNode<MarginContainer>(
			"UINode/MargContBR");

		instManual = "Press left & right arrows to change angle. " +
			"Press <Space> to simulate.";
		instSim = "Press <Space> to stop simulation and change " +
			"initial conditions.";

		instructLabel = new Label();
		mcBR.AddChild(instructLabel);
		instructLabel.Text = instManual;

	}
}
