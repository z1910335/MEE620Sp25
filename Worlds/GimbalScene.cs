//============================================================================
// GimbalScene.cs
//============================================================================
using Godot;
using System;

public partial class GimbalScene : Node3D
{
	String modeString = "YPR";
	bool configStrValid;
	String modeStr;
	String[] angNames;
	float[] angles;
	int actvIdx;
	float dTheta;
	bool dcmValid;

	// ModelStuff
	GimbalToy model;
	AirplaneToy model2;
	bool showGhost;

	// Sim stuff
	EulerKinemSim sim;
	double time;
	double rollRate;
	double yawRate;
	double pitchRate;
	double maxRate;

	enum OpMode
	{
		Manual,
		Simulate,
	}
	OpMode opMode;         // operation mode

	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	// Data display stuff
	//UIPanelDisplay datDisplay;
	int uiRefreshCtr;     //counter for display refresh
	int uiRefreshTHold;   // threshold for display refresh

	// UI input
	OptionButton eulerOptionButton;
	ButtonGroup buttonGroup;
	CheckBox rollSpinButton;
	CheckBox yawSpinButton;
	CheckBox pitchSpinButton;
	Button simButton;

	Label instructLabel;
	String instStr;

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("MEE 620 - Gimbal Scene");

		angNames = new string[3];
		angles = new float[3];
		actvIdx = 0;
		dTheta = 2.0f;
		dcmValid = true;

		configStrValid = SetConfig("YPR");

		float ctrHeight = 1.7f;
		model = GetNode<GimbalToy>("GimbalToy");
		model.Position = new Vector3(0.0f, ctrHeight, 0.0f);
		if(configStrValid)
			model.Setup(modeStr);

		model2 = GetNode<AirplaneToy>("AirplaneToy");
		model2.Position = new Vector3(0.0f, ctrHeight, 0.0f);
		model2.SetTransparency(0.8f);
		showGhost = false;
		model2.ShowCones();
		model2.Hide();
		model.SetRefModel(model2);

		opMode = OpMode.Manual;

		// set up the sim
		sim = new EulerKinemSim();
		sim.SetEulerType(modeStr);
		time = 0.0;
		rollRate = 1.0;  yawRate = 0.0;  pitchRate = 0.0;
		maxRate = 5.0;
		sim.RollRate = rollRate;
		sim.YawRate = yawRate;
		sim.PitchRate = pitchRate;
		sim.Theta1 = 0.0;
		sim.Theta2 = 0.0;
		sim.Theta3 = 0.0;

		// Set up the camera rig
		longitudeDeg = 30.0f;
		latitudeDeg = 15.0f;
		camDist = 10.0f;
		camFOV = 20.0f;

		camTg = new Vector3(0.0f, ctrHeight, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.FOVDeg = camFOV;
		cam.Target = camTg;

		// Set up data display
		// datDisplay = GetNode<UIPanelDisplay>(
		// 	"UINode/MarginContainer/DatDisplay");
		// datDisplay.SetNDisplay(9);

		// datDisplay.SetDigitsAfterDecimal(3, 1);
		// datDisplay.SetDigitsAfterDecimal(4, 1);
		// datDisplay.SetDigitsAfterDecimal(5, 1);
		// datDisplay.SetDigitsAfterDecimal(6, 1);
		// datDisplay.SetDigitsAfterDecimal(7, 1);
		// datDisplay.SetDigitsAfterDecimal(8, 1);

		// datDisplay.SetLabel(0,"Euler Angles");
		// datDisplay.SetValue(0,"");
		// datDisplay.SetLabel(1,"Mode");
		// datDisplay.SetValue(1, "Manual");
		// datDisplay.SetLabel(2,"Ghost Model");
		// datDisplay.SetValue(2,"OFF");
		// datDisplay.SetLabel(3, angNames[0] + ">>");
		// datDisplay.SetValue(3, angles[0]);
		// datDisplay.SetLabel(4, angNames[1]);
		// datDisplay.SetValue(4, angles[1]);
		// datDisplay.SetLabel(5, angNames[2]);
		// datDisplay.SetValue(5, angles[2]);
		// datDisplay.SetLabel(6, "omegaXB");
		// datDisplay.SetValue(6, 1.0f);
		// datDisplay.SetLabel(7, "omegaYB");
		// datDisplay.SetValue(7, 0.0f);
		// datDisplay.SetLabel(8, "omegaZB");
		// datDisplay.SetValue(8, 0.0f);

		// datDisplay.SetYellow(3);
		// datDisplay.SetCyan(6);
		// datDisplay.SetCyan(7);
		// datDisplay.SetCyan(8);
		uiRefreshCtr = 0;
		uiRefreshTHold = 3;

		// Euler option button
		eulerOptionButton = GetNode<OptionButton>(
			"UINode/MarginContainerTR/VBox/EulerOption");
		eulerOptionButton.AddItem("Yaw-Pitch-Roll",0);
		eulerOptionButton.AddItem("Roll-Yaw-Pitch",1);
		eulerOptionButton.ItemSelected += OnEulerSelection;

		// Button group
		rollSpinButton = GetNode<CheckBox>(
			"UINode/MarginContainerTR/VBox/CheckBoxRoll");
		yawSpinButton = GetNode<CheckBox>(
			"UINode/MarginContainerTR/VBox/CheckBoxYaw");
		pitchSpinButton = GetNode<CheckBox>(
			"UINode/MarginContainerTR/VBox/CheckBoxPitch");
		buttonGroup = rollSpinButton.ButtonGroup;
		GD.Print(buttonGroup.GetButtons());
		buttonGroup.Pressed += OnButtonGroupPressed;

		// Sim Button
		simButton = GetNode<Button>(
			"UINode/MarginContainerTR/VBox/SimButton");
		simButton.Pressed += OnSimButtonPress;

		// instruction label
		instructLabel = GetNode<Label>(
			"UINode/MarginContainerBL/InstructLabel");
		instStr = "Press <TAB> to switch angles; " +
			"arrow keys to increase/decrease angle " + 
			"(up/down for incremental control); or 0 to "+
			"zero the angle. Press G to toggle ghost plane";
		instructLabel.Text = instStr;
		//instructLabel.Set("theme_override_colors/font_color",new Color(1,1,0));
	}

    //------------------------------------------------------------------------
    // OnEulerSelection:   gets called when user selects a different Euler
    //                     angle type.
    //------------------------------------------------------------------------

    private void OnEulerSelection(long idx)
    {
		string modelStr = "YPR";
		switch(idx){
			case 0:
				modelStr = "YPR";
				break;
			case 1:
				modelStr = "RYP";
				break;
			default:
				modelStr = "YPR";
				break;
		}

		model.SetAngles(0.0f, 0.0f, 0.0f);
		SetConfig(modelStr);
		model.Setup(modelStr);

		sim.SetEulerType(modelStr);

		actvIdx = 0;
		// datDisplay.SetLabel(3, angNames[0] + ">>");
		// datDisplay.SetLabel(4, angNames[1]);
		// datDisplay.SetLabel(5, angNames[2]);
		// datDisplay.SetValue(3, 0.0f);
		// datDisplay.SetValue(4, 0.0f);
		// datDisplay.SetValue(5, 0.0f);
		// datDisplay.SetYellow(3);
		// datDisplay.SetWhite(4);
		// datDisplay.SetWhite(5);

		GD.Print("Euler Mode Selected: " + modelStr);
    }

	//------------------------------------------------------------------------
	// OnButtonGroupPressed:
	//------------------------------------------------------------------------
	private void OnButtonGroupPressed(BaseButton button)
	{
		//GD.Print("Button Group Pressed");
		if(rollSpinButton.ButtonPressed){
			//GD.Print("rollSpinButton is pressed");
			rollRate = 1.0;  yawRate = pitchRate = 0.0;
		}
		else if(yawSpinButton.ButtonPressed){
			//GD.Print("yawSpinButton is pressed");
			yawRate = 1.0;  rollRate = pitchRate = 0.0;
		}
		else if(pitchSpinButton.ButtonPressed){
			//GD.Print("pitchSpinButton is pressed");
			pitchRate = 1.0; rollRate = yawRate = 0.0;
		}

		// datDisplay.SetValue(6, (float)rollRate);
		// datDisplay.SetValue(7, (float)yawRate);
		// datDisplay.SetValue(8, (float)pitchRate);
	}


	//------------------------------------------------------------------------
	// OnSimButtonPress
	//------------------------------------------------------------------------
	private void OnSimButtonPress()
	{
		//GD.Print("SimButton Pressed");
		if(opMode == OpMode.Manual){
			rollSpinButton.Disabled = true;
			yawSpinButton.Disabled = true;
			pitchSpinButton.Disabled = true;
			eulerOptionButton.Disabled = true;

			sim.RollRate  = rollRate;
			sim.YawRate   = yawRate;
			sim.PitchRate = pitchRate;
			sim.Theta1 = (double)Mathf.DegToRad(angles[0]);
			sim.Theta2 = (double)Mathf.DegToRad(angles[1]);
			sim.Theta3 = (double)Mathf.DegToRad(angles[2]);

			opMode = OpMode.Simulate;
			// datDisplay.SetValue(1, "Simulate");
			// datDisplay.SetWhite(3);
			// datDisplay.SetWhite(4);
			// datDisplay.SetWhite(5);
			simButton.Text = "STOP Sim";
		}
		else{

			rollSpinButton.Disabled = false;
			yawSpinButton.Disabled = false;
			pitchSpinButton.Disabled = false;
			eulerOptionButton.Disabled = false;

			opMode = OpMode.Manual;
			actvIdx = 0;
			// datDisplay.SetValue(1, "Manual");
			// datDisplay.SetLabel(3, angNames[0] + " >>");
			// datDisplay.SetYellow(3);
			simButton.Text = "Simulate";
		}
	}

	//------------------------------------------------------------------------
	// SetConfig
	//------------------------------------------------------------------------
	private bool SetConfig(String mm)
	{
		String mStr = mm.ToUpper();
		if(mStr == "YPR"){
			modeStr = "YPR";
			angNames[0] = "Yaw (deg)";
			angNames[1] = "Pitch (deg)";
			angNames[2] = "Roll (deg)";
			angles[0] = angles[1] = angles[2] = 0.0f;
			return true;
		}
		else if(mStr == "RYP"){
			modeStr = "RYP";
			angNames[0] = "Roll (deg)";
			angNames[1] = "Yaw (deg)";
			angNames[2] = "Pitch (deg)";
			angles[0] = angles[1] = angles[2] = 0.0f;
			return true;
		}

		mStr = "ERROR";
		angNames[0] = angNames[1] = angNames[2] = "ERROR";
		return false;
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
		if(opMode == OpMode.Simulate){
			double th1 = sim.Theta1;
			double th2 = sim.Theta2;
			double th3 = sim.Theta3;

			if(th1 > Math.PI){
				th1 -= 2.0*Math.PI;
				sim.Theta1 = th1;
			}
			if(th1 < -Math.PI){
				th1 += 2.0*Math.PI;
				sim.Theta1 = th1;
			}
			if(th2 > Math.PI){
				th2 -= 2.0*Math.PI;
				sim.Theta2 = th2;
			}
			if(th2 < -Math.PI){
				th2 += 2.0*Math.PI;
				sim.Theta2 = th2;
			}
			if(th3 > Math.PI){
				th3 -= 2.0*Math.PI;
				sim.Theta3 = th3;
			}
			if(th3 < -Math.PI){
				th3 += 2.0*Math.PI;
				sim.Theta3 = th3;
			}


			model.SetAngles((float)sim.Theta1, (float)sim.Theta2, 
				(float)sim.Theta3);

			if(uiRefreshCtr > uiRefreshTHold){
				angles[0] = Mathf.RadToDeg((float)sim.Theta1);
				angles[1] = Mathf.RadToDeg((float)sim.Theta2);
				angles[2] = Mathf.RadToDeg((float)sim.Theta3);

				// datDisplay.SetValue(3,angles[0]);
				// datDisplay.SetValue(4,angles[1]);
				// datDisplay.SetValue(5,angles[2]);
				
				uiRefreshCtr = 0;
			}
			++uiRefreshCtr;

			return;
		}


		bool angleChanged = false;

		if(Input.IsActionPressed("ui_right")){
			angles[actvIdx] += dTheta;
			angleChanged = true;
		}

		if(Input.IsActionPressed("ui_left")){
			angles[actvIdx] -= dTheta;
			angleChanged = true;
		}

		if(Input.IsActionJustPressed("ui_up")){
			angles[actvIdx] += dTheta;
			angleChanged = true;
		}

		if(Input.IsActionJustPressed("ui_down")){
			angles[actvIdx] -= dTheta;
			angleChanged = true;
		}

		if(Input.IsActionJustPressed("ui_zero")){
			angles[actvIdx]  = 0.0f;
			angleChanged = true;
		}

		if(angleChanged){
			// datDisplay.SetValue(actvIdx+3, angles[actvIdx]);
			if(angles[actvIdx] > 180.0f)
				angles[actvIdx] -= 360.0f;
			if(angles[actvIdx] < -180.0f)
				angles[actvIdx] += 360.0f;
			ProcessAngleChange();
		}

		if(Input.IsActionJustPressed("ui_focus_next")){
			// datDisplay.SetLabel(actvIdx+3, angNames[actvIdx]);
			// datDisplay.SetWhite(actvIdx+3);
			++actvIdx;
			if(actvIdx >2)
				actvIdx = 0;
			// datDisplay.SetLabel(actvIdx+3, angNames[actvIdx]+">>");
			// datDisplay.SetYellow(actvIdx+3);
		}

		if(Input.IsActionJustPressed("ui_ghost")){
			if(showGhost){
				showGhost = false;
				model2.Hide();
				// datDisplay.SetValue(2,"OFF");
				// datDisplay.SetWhite(2);
			}
			else{
				showGhost = true;
				model2.Show();
				// datDisplay.SetValue(2,"ON");
				// datDisplay.SetWhite(2);
				ProcessAngleChange();
			}
		}
	}

	//------------------------------------------------------------------------
	// ProcessAngleChange
	//------------------------------------------------------------------------
	private void ProcessAngleChange()
	{
		int rCode = model.SetAngles(Mathf.DegToRad(angles[0]), 
				Mathf.DegToRad(angles[1]), Mathf.DegToRad(angles[2]));
			if(rCode == 2){ // bad DCM
				if(dcmValid && showGhost){
					dcmValid = false;
					// datDisplay.SetCyan(2,false,true);
					// datDisplay.SetValue(2,"ERROR!");
				}
			}
			else{
				if(!dcmValid && showGhost){
					dcmValid = true;
					// datDisplay.SetWhite(2);
					// datDisplay.SetValue(2,"ON");
				}
			}
	}

    //------------------------------------------------------------------------
    // _PhysicsProcess
    //------------------------------------------------------------------------
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		if(opMode != OpMode.Simulate){
			return;
		}

		double deltaByTwo = 0.5*delta;
		sim.Step(time, deltaByTwo);
		time += deltaByTwo;
		sim.Step(time, deltaByTwo);
		time += deltaByTwo;
    }
}
