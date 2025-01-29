//============================================================================
// PendCartScene.cs
// Code for runing the Godot scene of a planar planar pendulum attached
// a translating cart.
//============================================================================
using Godot;
using System;

public partial class PendCartScene : Node3D
{
	// exported system parameters
	[Export]
	double cartMass = 1.9;
	[Export]
	double pendMass = 2.8; 

	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	// model stuff
	PendCartModel model;

	// Data display stuff
	//UIPanelDisplay datDisplay;
	GridIO gridIO;
	int uiRefreshCtr;     //counter for display refresh
	int uiRefreshTHold;   // threshold for display refresh

	// Mode of operation
	enum OpMode
	{
		SetCart,
		SetAngle,
		Simulate,
	}

	OpMode opMode;         // operation mode
	float cartX;           // position of cart
	float pendAngle;       // angle of pendulum
	float dxMan;       // amount cart position is changed manually
	float dthetaMan;   // amount angle is changed each time updated manually
	bool manChanged;   // position or angle has been changed manually

	PendCartSim sim;       // the simulaion
	double time;           // self explanitory

	Label instructLabel;   // label to display instructions
	String instManPos;     // instructions when in SetPosition mode
	String instManAngle;   // instructions when in SetAngle mode
	String instSim;        // instructions when in Simulate model

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("Pendulum Cart Scene");

		// build the simulation
		float wallHeight = 2.0f;
		double pendLength = 1.5;
		opMode = OpMode.SetCart;
		dxMan = 0.02f;
		dthetaMan = 0.03f;
		manChanged = false;
		sim = new PendCartSim();
		sim.PendulumLength = pendLength;
		sim.PendulumMass = pendMass;
		sim.CartMass = cartMass;
		sim.Position = 0.0;
		sim.Angle = 0.0;
		sim.GenSpeedCart = 0.0;
		sim.GenSpeedPend = 0.0;
		time = 0.0;

		// build the model
		model = GetNode<PendCartModel>("PendCartModel");
		model.Position = new Vector3(0.0f, wallHeight, 0.0f);
		model.PendulumLength = (float)pendLength;

		// Set up the camera rig
		longitudeDeg = 30.0f;
		latitudeDeg = 15.0f;
		camDist = 4.0f;
		camFOV = 55.0f;

		camTg = new Vector3(0.0f, wallHeight, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.FOVDeg = camFOV;
		cam.Target = camTg;

		SetupUI();
		
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{

		if(opMode == OpMode.SetCart){
			if(Input.IsActionPressed("ui_right")){
				cartX += dxMan;
				model.SetPositionAngle(cartX, pendAngle);
				manChanged = true;


				// datDisplay.SetValue(1, cartX);
				// datDisplay.SetValue(3, "---");
				// datDisplay.SetValue(4, "---");
				// datDisplay.SetValue(5, "---");
				// datDisplay.SetValue(6, "---");
				// datDisplay.SetValue(7, "---");
			}

			if(Input.IsActionPressed("ui_left")){
				cartX -= dxMan;
				model.SetPositionAngle(cartX, pendAngle);
				manChanged = true;

				// datDisplay.SetValue(1, cartX);
				// datDisplay.SetValue(3, "---");
				// datDisplay.SetValue(4, "---");
				// datDisplay.SetValue(5, "---");
				// datDisplay.SetValue(6, "---");
				// datDisplay.SetValue(7, "---");
			}

			if(Input.IsActionJustPressed("ui_focus_next")){
				opMode = OpMode.SetAngle;
				// datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instManAngle;
			}

			if(Input.IsActionJustPressed("ui_accept")){
				if(manChanged){
					sim.Position = (double)cartX;
					sim.Angle = (double)pendAngle;
					sim.GenSpeedCart = 0.0;
					sim.GenSpeedPend = 0.0;
				}

				opMode = OpMode.Simulate;
				// datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instSim;
				manChanged = false;
			}
			return;
		} // if opMode.SetPosition

		if(opMode == OpMode.SetAngle) {
			if(Input.IsActionPressed("ui_right")){
				pendAngle += dthetaMan;
				model.SetPositionAngle(cartX, pendAngle);
				manChanged = true;

				// datDisplay.SetValue(2, Mathf.RadToDeg(pendAngle));
				// datDisplay.SetValue(3, "---");
				// datDisplay.SetValue(4, "---");
				// datDisplay.SetValue(5, "---");
				// datDisplay.SetValue(6, "---");
				// datDisplay.SetValue(7, "---");
			}

			if(Input.IsActionPressed("ui_left")){
				pendAngle -= dthetaMan;
				model.SetPositionAngle(cartX, pendAngle);
				manChanged = true;

				// datDisplay.SetValue(2, Mathf.RadToDeg(pendAngle));
				// datDisplay.SetValue(3, "---");
				// datDisplay.SetValue(4, "---");
				// datDisplay.SetValue(5, "---");
				// datDisplay.SetValue(6, "---");
				// datDisplay.SetValue(7, "---");
			}

			if(Input.IsActionJustPressed("ui_focus_next")){
				opMode = OpMode.SetCart;
				// datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instManPos;
			}

			if(Input.IsActionJustPressed("ui_accept")){
				if(manChanged){
					sim.Position = (double)cartX;
					sim.Angle = (double)pendAngle;
					sim.GenSpeedCart = 0.0;
					sim.GenSpeedPend = 0.0;
				}

				opMode = OpMode.Simulate;
				// datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instSim;
				manChanged = false;
			}
			return;
		} // if OpMode.SetAngle

		// update model position and angle
		cartX = (float)sim.Position;
		pendAngle = (float)sim.Angle;
		model.SetPositionAngle(cartX, pendAngle);
		
		// data display
		if(uiRefreshCtr > uiRefreshTHold){
			float ke = (float)sim.KineticEnergy;
			float pe = (float)sim.PotentialEnergy;
			float xG = (float)sim.MassCenterX;
			float yG = (float)sim.MassCenterY;

			// datDisplay.SetValue(1, cartX);
			// datDisplay.SetValue(2, Mathf.RadToDeg(pendAngle));
			// datDisplay.SetValue(3, ke);
			// datDisplay.SetValue(4, pe);
			// datDisplay.SetValue(5, ke+pe);
			// datDisplay.SetValue(6, xG);
			// datDisplay.SetValue(7, yG);
			uiRefreshCtr = 0;   // reset the counter
		}
		++uiRefreshCtr;

		if(opMode == OpMode.Simulate){
			if(Input.IsActionJustPressed("ui_accept")){
				opMode = OpMode.SetCart;
				// datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instManPos;
			}
		} // end if OpMode.Simulate

	} // end _Process

	//------------------------------------------------------------------------
    // _PhysicsProcess:
    //------------------------------------------------------------------------
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		if(opMode != OpMode.Simulate)
			return;

		double deltaByTwo = 0.5*delta;
		sim.Step(time, deltaByTwo);
		time += deltaByTwo;
		sim.Step(time, deltaByTwo);
		time += deltaByTwo;
	}

	//------------------------------------------------------------------------
	// SetupUI
	//------------------------------------------------------------------------
	private void SetupUI()
	{
		MarginContainer mcTL = GetNode<MarginContainer>(
			"UINode/MarginContainer");

		VBoxContainer vBoxTL = new VBoxContainer();

		PanelContainer pc = new PanelContainer();
		mcTL.AddChild(pc);
		pc.AddChild(vBoxTL);

		Label title = new Label();
		title.Text = "Pendulum & Cart";
		vBoxTL.AddChild(title);

		vBoxTL.AddChild(new HSeparator());

		gridIO = new GridIO();
		vBoxTL.AddChild(gridIO);
		gridIO.SetSize(6,2);
		gridIO.InitGridCells();

		gridIO.SetDigitsAfterDecimal(1, 1, 2);
		gridIO.SetDigitsAfterDecimal(2, 1, 2);
		gridIO.SetDigitsAfterDecimal(3, 1, 4);
		gridIO.SetDigitsAfterDecimal(4, 1, 4);
		gridIO.SetDigitsAfterDecimal(5, 1, 4);

		gridIO.SetText(0,0, "Mode: ");
		gridIO.SetText(1,0, "Cart Pos:");
		gridIO.SetText(2,0, "Angle (deg):");
		gridIO.SetText(3,0, "Kinetic:");
		gridIO.SetText(4,0, "Potential:");
		gridIO.SetText(5,0, "Tot Energy:");

		gridIO.SetText(0,1, "SetCart");
		gridIO.SetText(1,1, "----");
		gridIO.SetText(2,1, "----");
		gridIO.SetText(3,1, "----");
		gridIO.SetText(4,1, "----");
		gridIO.SetText(5,1, "----");

		uiRefreshCtr = 0;
		uiRefreshTHold = 3;

		instructLabel = GetNode<Label>(
			"UINode/MarginContainerBL/InstructLabel");
		instManPos = "Press left & right arrows to change cart position; " +
			"<Tab> to change pendulum angle; <Space> to simulate.";
		instManAngle = "Press left & right arrows to change pendulum angle; "+
			"<Tab> to change cart position; <Space> to simulate.";
		instSim = "Press <Space> to stop simulation and change " +
			"initial conditions.";
		instructLabel.Text = instManPos;
	}
}
