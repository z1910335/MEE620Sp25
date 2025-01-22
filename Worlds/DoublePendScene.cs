//============================================================================
// DoublePendScene.cs
// Code for runing the Godot scene of a planar double pendulum
//============================================================================
using Godot;
using System;

public partial class DoublePendScene : Node3D
{
	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	// Model stuff
	StickBall pModel1;
	StickBall pModel2;
	double pendLen1;
	double pendLen2;
	double pendMass1;
	double pendMass2;

	// Data display stuff
	// UIPanelDisplay datDisplay;
	int uiRefreshCtr;     //counter for display refresh
	int uiRefreshTHold;   // threshold for display refresh

	// Mode of operation
	enum OpMode
	{
		Manual_1,
		Manual_2,
		Sim
	}

	OpMode opMode;         // operation mode
	Vector3 pend1Rotation; // rotation value for pendulum 1
	Vector3 pend2Rotation; // rotation value for pendulum 2
	float dthetaMan;   // amount angle is changed each time updated manually
	bool angleManChanged;  // Angle has been changed manually

	DoublePendSim sim;     // the simulation
	double time;

	Label instructLabel;   // label to display instructions
	String instManual1;    // instructions when in manual_1 mode
	String instManual2;    // instructions when in manual_2 mode
	String instSim;        // instructions when in sim model

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("Double Pendulum Scene");

		// build the simulation
		pendLen1 = 0.9;
		pendLen2 = 0.7;
		pendMass1 = 1.4;
		pendMass2 = 1.1;
		opMode = OpMode.Manual_1;
		pend1Rotation = new Vector3();
		pend2Rotation = new Vector3();
		dthetaMan = 0.03f;
		angleManChanged = true;
		sim = new DoublePendSim();
		sim.Length1 = pendLen1;
		sim.Length2 = pendLen2;
		sim.Mass1 = pendMass1;
		sim.Mass2 = pendMass2;
		sim.Angle1 = 0.0;
		sim.Angle2 = 0.0;
		sim.GenSpeed1 = 0.0;
		sim.GenSpeed2 = 0.0;
		time = 0.0;

		// build the model
		float mountHeight = 1.9f;
		Node3D mnt = GetNode<Node3D>("Axle");
		mnt.Position = new Vector3(0.0f, mountHeight, 0.0f);
		var sbScene = GD.Load<PackedScene>("res://Models/StickBall.tscn");
		pModel1 = (StickBall)sbScene.Instantiate();
		AddChild(pModel1);
		pModel1.Position = new Vector3(0.0f, mountHeight, 0.0f);
		pModel1.Length = (float)pendLen1;
		pModel1.BallDiameter = 0.25f;

		pModel2 = (StickBall)sbScene.Instantiate();
		pModel1.AddChild(pModel2);
		pModel2.Position = new Vector3(0.0f, -(float)pendLen1, 0.0f);
		pModel2.Length = (float)pendLen2;
		pModel2.BallDiameter = 0.25f;

		// Set up the camera rig
		longitudeDeg = 30.0f;
		latitudeDeg = 15.0f;
		camDist = 2.7f;

		camTg = new Vector3(0.0f, mountHeight, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.Target = camTg;

		// Set up data display
		// datDisplay = GetNode<UIPanelDisplay>(
		// 	"UINode/MarginContainer/DatDisplay");
		// datDisplay.SetNDisplay(6);
		// datDisplay.SetLabel(0,"Mode");
		// datDisplay.SetValue(0,opMode.ToString());
		// datDisplay.SetLabel(1,"Angle1 (deg)");
		// datDisplay.SetValue(1, 0.0f);
		// datDisplay.SetLabel(2,"Angle2 (deg)");
		// datDisplay.SetValue(2, 0.0f);
		// datDisplay.SetLabel(3,"Kinetic");
		// datDisplay.SetValue(3,"---");
		// datDisplay.SetLabel(4,"Potential");
		// datDisplay.SetValue(4,"---");
		// datDisplay.SetLabel(5,"Tot Energy");
		// datDisplay.SetValue(5,"---");

		// datDisplay.SetDigitsAfterDecimal(1,1);
		// datDisplay.SetDigitsAfterDecimal(2,1);
		// datDisplay.SetDigitsAfterDecimal(3,4);
		// datDisplay.SetDigitsAfterDecimal(4,4);
		// datDisplay.SetDigitsAfterDecimal(5,4);

		uiRefreshCtr = 0;
		uiRefreshTHold = 3;

		instructLabel = GetNode<Label>(
			"UINode/MarginContainerBL/InstructLabel");
		instManual1 = "Press left & right arrows to change angle 1; " +
			"<Tab> to change angle 2; <Space> to simulate.";
		instManual2 = "Press left & right arrows to change angle 2; " +
			"<Tab> to change angle 1; <Space> to simulate.";
		instSim = "Press <Space> to stop simulation and change " +
			"initial conditions.";
		instructLabel.Text = instManual1;
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
		if(sim.Angle1 > Math.PI)
			sim.Angle1 = sim.Angle1 - 2.0*Math.PI;
		if(sim.Angle1 < -Math.PI)
			sim.Angle1 = sim.Angle1 + 2.0*Math.PI;
		if(sim.Angle2 > Math.PI)
			sim.Angle2 = sim.Angle2 - 2.0*Math.PI;
		if(sim.Angle2 < -Math.PI)
			sim.Angle2 = sim.Angle2 + 2.0*Math.PI;

		if(opMode == OpMode.Manual_1){  // change angle manually
			if(Input.IsActionPressed("ui_right")){
				pend1Rotation.Z += dthetaMan;
				// datDisplay.SetValue(1, Mathf.RadToDeg(pend1Rotation.Z));
				// datDisplay.SetValue(3, "---");
				// datDisplay.SetValue(4, "---");
				// datDisplay.SetValue(5, "---");
				pModel1.Rotation = pend1Rotation;
				angleManChanged = true;
			}
			if(Input.IsActionPressed("ui_left")){
				pend1Rotation.Z -= dthetaMan;
				// datDisplay.SetValue(1, Mathf.RadToDeg(pend1Rotation.Z));
				// datDisplay.SetValue(3, "---");
				// datDisplay.SetValue(4, "---");
				// datDisplay.SetValue(5, "---");
				pModel1.Rotation = pend1Rotation;
				angleManChanged = true;
			}

			if(Input.IsActionJustPressed("ui_focus_next")){
				opMode = OpMode.Manual_2;
				// datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instManual2;
			}

			if(Input.IsActionJustPressed("ui_accept")){
				if(angleManChanged){
					sim.Angle1 = (double)pend1Rotation.Z;
					sim.Angle2 = (double)pend2Rotation.Z;
					sim.GenSpeed1 = 0.0;
					sim.GenSpeed2 = 0.0;
				}

				opMode = OpMode.Sim;
				// datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instSim;
				angleManChanged = false;
			}

			return;
		}

		if(opMode == OpMode.Manual_2){  // change angle 2 manually
			if(Input.IsActionPressed("ui_right")){
				pend2Rotation.Z += dthetaMan;
				// datDisplay.SetValue(2, Mathf.RadToDeg(pend2Rotation.Z));
				// datDisplay.SetValue(3, "---");
				// datDisplay.SetValue(4, "---");
				// datDisplay.SetValue(5, "---");
				pModel2.Rotation = pend2Rotation;
				angleManChanged = true;
			}
			if(Input.IsActionPressed("ui_left")){
				pend2Rotation.Z -= dthetaMan;
				// datDisplay.SetValue(2, Mathf.RadToDeg(pend2Rotation.Z));
				// datDisplay.SetValue(3, "---");
				// datDisplay.SetValue(4, "---");
				// datDisplay.SetValue(5, "---");
				pModel2.Rotation = pend2Rotation;
				angleManChanged = true;
			}

			if(Input.IsActionJustPressed("ui_focus_next")){
				opMode = OpMode.Manual_1;
				// datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instManual1;
			}

			if(Input.IsActionJustPressed("ui_accept")){
				if(angleManChanged){
					sim.Angle1 = (double)pend1Rotation.Z;
					sim.Angle2 = (double)pend2Rotation.Z;
					sim.GenSpeed1 = 0.0;
					sim.GenSpeed2 = 0.0;
				}

				opMode = OpMode.Sim;
				// datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instSim;
				angleManChanged = false;
			}

			return;
		} // if OpMode.Manual_2

		// rotate the model links
		pend1Rotation.Z = (float)sim.Angle1;
		pend2Rotation.Z = (float)sim.Angle2;
		pModel1.Rotation = pend1Rotation;
		pModel2.Rotation = pend2Rotation;

		// data display
		if(uiRefreshCtr > uiRefreshTHold){
			float ke = (float)sim.KineticEnergy;
			float pe = (float)sim.PotentialEnergy;

			// datDisplay.SetValue(1, Mathf.RadToDeg(pend1Rotation.Z));
			// datDisplay.SetValue(2, Mathf.RadToDeg(pend2Rotation.Z));
			// datDisplay.SetValue(3, ke);
			// datDisplay.SetValue(4, pe);
			// datDisplay.SetValue(5, ke+pe);
			uiRefreshCtr = 0;   // reset the counter
		}
		++uiRefreshCtr;

		if(opMode == OpMode.Sim){
			if(Input.IsActionJustPressed("ui_accept")){
				opMode = OpMode.Manual_1;
				// datDisplay.SetValue(0, opMode.ToString());
				instructLabel.Text = instManual1;
			}
		} // end if OpMode.Sim
	}

	//------------------------------------------------------------------------
    // _PhysicsProcess:
    //------------------------------------------------------------------------
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		if(opMode != OpMode.Sim)
			return;

		sim.Step(time, delta);
		time += delta;
	}
}
