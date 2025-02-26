//============================================================================
// SpinTopScene.cs  
//============================================================================
using Godot;
using System;

public partial class SpinTopScene : Node3D
{

	enum OpMode
	{
		Configure,
		Simulate,
	}
	OpMode opMode;         // operation mode

	bool toPrecess;        // whether to choose IC for simple precession

	// Simulation
	SpinTopSim sim;
	double time;
	int nSimSteps;         // number of sim steps per _PhysicsProcess
	int simOpt;            // integer corresponding to simulation type
						   // 0=body fixed;  1=lean rame

	// UI
	Button[] adjButtons;
	float leanICDeg;       // initial lean angle in degrees
	float leanICMin;
	float leanICMax;
	float dAngle;

	OptionButton optionSimSubsteps;
	OptionButton optionSim;
	OptionButton optionSpinRate;
	CheckBox checkPrecess;

	double[] spinRateOptions;
	string[,] spinRateLabelOptions; 
	int spinRateIdx;

	double spinRate;       // initial spinRate;
	float dumAngle;

	// Model
	TopDiskModel model;    // model

	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	// Data display stuff
	//UIPanelDisplay datDisplay;
	GridIO gridIO;
	int uiRefreshCtr;     //counter for display refresh
	int uiRefreshTHold;   // threshold for display refresh

	// More UI stuff
	Button simButton;

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		
		opMode = OpMode.Configure;
		toPrecess = false;

		// ui
		leanICDeg = 30.0f;
		leanICMin = 5.0f;
		leanICMax = 170.0f;
		dAngle = 1.0f;

		spinRateOptions = new double[5];
		spinRateOptions[0] = 10.0;
		spinRateOptions[1] = 20.0;
		spinRateOptions[2] = 30.0;
		spinRateOptions[3] = 40.0;
		spinRateOptions[4] = 60.0;

		spinRateLabelOptions = new string[2,5];
		spinRateLabelOptions[0,0] = "OmegaY: 10 rad/s";
		spinRateLabelOptions[0,1] = "OmegaY: 20 rad/s";
		spinRateLabelOptions[0,2] = "OmegaY: 30 rad/s";
		spinRateLabelOptions[0,3] = "OmegaY: 40 rad/s";
		spinRateLabelOptions[0,4] = "OmegaY: 60 rad/s";
		spinRateLabelOptions[1,0] = "thetaDot: 10 rad/s";
		spinRateLabelOptions[1,1] = "thetaDot: 20 rad/s";
		spinRateLabelOptions[1,2] = "thetaDot: 30 rad/s";
		spinRateLabelOptions[1,3] = "thetaDot: 40 rad/s";
		spinRateLabelOptions[1,4] = "thetaDot: 60 rad/s";

		spinRateIdx = 4;

		// set up the model
		model = GetNode<TopDiskModel>("TopDiskModel");
		dumAngle = 0.0f;
		model.SetEulerAnglesYZY(0.0f, Mathf.DegToRad(leanICDeg), 0.0f);

		// Set up the simulation
		sim = new SpinTopSim();
		simOpt = 0;
		spinRate = spinRateOptions[spinRateIdx];
		sim.LeanAngle = Mathf.DegToRad(leanICDeg);
		sim.SpinRate = spinRate;

		nSimSteps = 16;
		time = 0.0;

		// Set up the camera rig
		longitudeDeg = 20.0f;
		latitudeDeg = 25.0f;
		camDist = 5.0f;
		camFOV = 35.0f;

		camTg = new Vector3(0.0f, 1.0f, 0.0f);
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
		if(opMode == OpMode.Simulate){
			
			model.SetEulerAnglesYZY((float)sim.PrecessionAngle,
				(float)sim.LeanAngle, (float)sim.SpinAngle);

			if(uiRefreshCtr > uiRefreshTHold){
				sim.PostProcess();

				float leanDeg = Mathf.RadToDeg((float)sim.LeanAngle);
				double ke = sim.KineticEnergy;
				double pe = sim.PotentialEnergy;
				double totErg = ke + pe;

				double angMoY = sim.AngMoY;

				gridIO.SetNumeric(1,1, leanDeg);
				gridIO.SetNumeric(2,1, (float)ke);
				gridIO.SetNumeric(3,1, (float)pe);
				gridIO.SetNumeric(4,1, (float)totErg);
				gridIO.SetNumeric(5,1, (float)angMoY);

				// datDisplay.SetValue(1,leanDeg);
				// datDisplay.SetValue(2,(float)ke);
				// datDisplay.SetValue(3,(float)pe);
				// datDisplay.SetValue(4,(float)totErg);
				// datDisplay.SetValue(5,(float)angMoY);

				uiRefreshCtr = 0;
			}
			++uiRefreshCtr;

			return;
		}

		if(adjButtons[0].ButtonPressed){
            leanICDeg += dAngle;
            ProcessLeanAngle();
        }
        else if(adjButtons[3].ButtonPressed){
            leanICDeg -= dAngle;
            ProcessLeanAngle();
        }
	}

	//------------------------------------------------------------------------
	// _PhysicsProcess:
	//------------------------------------------------------------------------
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		if(opMode != OpMode.Simulate)
			return;

		double subdelta = delta/((double)nSimSteps);
		for(int i=0;i<nSimSteps;++i){
			sim.Step(time, subdelta);
			time += subdelta;
		}
    }

	//------------------------------------------------------------------------
	// ProcessLeanAngle
	//------------------------------------------------------------------------
	private void ProcessLeanAngle()
	{
		//GD.Print("Process Lean Angle");

		if(leanICDeg < leanICMin)
			leanICDeg = leanICMin;

		if(leanICDeg > leanICMax)
			leanICDeg = leanICMax;

		sim.ResetIC((double)Mathf.DegToRad(leanICDeg), spinRate, 
			checkPrecess.ButtonPressed);
		if(simOpt == 1 && checkPrecess.ButtonPressed){
			if(sim.PrecessICFound){
				checkPrecess.Set("theme_override_colors/font_pressed_color",
					new Color(0.45f,1.0f,0.45f));
				checkPrecess.Set("theme_override_colors/font_hover_pressed_color",
					new Color(0.45f,1.0f,0.45f));
				checkPrecess.Set("theme_override_colors/font_disabled_color",
					new Color(0.45f,1.0f,0.45f, 0.5f));
				//GD.Print("Ding");
			}
			else{
				checkPrecess.Set("theme_override_colors/font_pressed_color",
					new Color(1.0f,0.45f,0.45f));
				checkPrecess.Set("theme_override_colors/font_hover_pressed_color",
					new Color(1.0f,0.45f,0.45f));
				checkPrecess.Set("theme_override_colors/font_disabled_color",
					new Color(1.0f,0.45f,0.45f, 0.5f));
				//GD.Print("Dong");
			}
		}
		model.SetEulerAnglesYZY(0.0f,Mathf.DegToRad(leanICDeg), 0.0f);
		gridIO.SetNumeric(0,1, leanICDeg);
		gridIO.SetNumeric(1,1, leanICDeg);
		// datDisplay.SetValue(0, leanICDeg);
		// datDisplay.SetValue(1, leanICDeg);
	}

	//------------------------------------------------------------------------
	// SetupUI
	//------------------------------------------------------------------------
	private void SetupUI()
	{
		VBoxContainer vbox = GetNode<VBoxContainer>("UINode/MgContainTL/VBox");
		MarginContainer hd = vbox.GetNode<MarginContainer>("HoldsDisplay");

		// Set up data display
		gridIO = new GridIO();
		hd.AddChild(gridIO);
		gridIO.SetSize(6,2);
		gridIO.InitGridCells();

		gridIO.SetDigitsAfterDecimal(0,1,1);
		gridIO.SetDigitsAfterDecimal(1,1,1);
		gridIO.SetDigitsAfterDecimal(2,1,4);
		gridIO.SetDigitsAfterDecimal(3,1,4);
		gridIO.SetDigitsAfterDecimal(4,1,4);
		gridIO.SetDigitsAfterDecimal(5,1,4);

		gridIO.SetText(0,0,"Lean IC");
		gridIO.SetText(1,0,"Lean deg");
		gridIO.SetText(2,0,"Kinetic");
		gridIO.SetText(3,0,"Potential");
		gridIO.SetText(4,0,"Total");
		gridIO.SetText(5,0,"Ang.Mo.Vert");

		gridIO.SetNumeric(0,1, leanICDeg);
		gridIO.SetNumeric(1,1, leanICDeg);
		gridIO.SetNumeric(2,1, 0.0f);
		gridIO.SetNumeric(3,1, 0.0f);
		gridIO.SetNumeric(4,1, 0.0f);
		gridIO.SetNumeric(5,1, 0.0f);

		// datDisplay = vbox.GetNode<UIPanelDisplay>("DatDisplay");
		// datDisplay.SetNDisplay(6);

		// datDisplay.SetDigitsAfterDecimal(0,1);
		// datDisplay.SetDigitsAfterDecimal(1,1);
		// datDisplay.SetDigitsAfterDecimal(2,4);
		// datDisplay.SetDigitsAfterDecimal(3,4);
		// datDisplay.SetDigitsAfterDecimal(4,4);
		// datDisplay.SetDigitsAfterDecimal(5,4);

		// datDisplay.SetLabel(0,"Lean IC");
		// datDisplay.SetLabel(1,"Lean deg");
		// datDisplay.SetLabel(2,"Kinetic");
		// datDisplay.SetLabel(3,"Potential");
		// datDisplay.SetLabel(4,"Total");
		// datDisplay.SetLabel(5,"Ang.Mo.Vert");

		// datDisplay.SetValue(0,leanICDeg);
		// datDisplay.SetValue(1,leanICDeg);
		// datDisplay.SetValue(2,0.0f);
		// datDisplay.SetValue(3,0.0f);
		// datDisplay.SetValue(4,0.0f);
		// datDisplay.SetValue(5,0.0f);

		uiRefreshCtr = 0;
		uiRefreshTHold = 3;

		//--- Option Button, Sim Substeps
		optionSimSubsteps = vbox.GetNode<OptionButton>("OptionSimSubSteps");
		optionSimSubsteps.AddItem("Substeps: 1",0);
		optionSimSubsteps.AddItem("Substeps: 2",1);
		optionSimSubsteps.AddItem("Substeps: 4",2);
		optionSimSubsteps.AddItem("Substeps: 8",3);
		optionSimSubsteps.AddItem("Substeps: 16",4);
		optionSimSubsteps.Selected = 4;
		optionSimSubsteps.ItemSelected += OnOptionSimSubsteps;

		//--- Option Button Spin Rate
		optionSpinRate = vbox.GetNode<OptionButton>("OptionSpinRate");
		optionSpinRate.AddItem(spinRateLabelOptions[0,0], 0);
		optionSpinRate.AddItem(spinRateLabelOptions[0,1], 1);
		optionSpinRate.AddItem(spinRateLabelOptions[0,2], 2);
		optionSpinRate.AddItem(spinRateLabelOptions[0,3], 3);
		optionSpinRate.AddItem(spinRateLabelOptions[0,4], 4);
		optionSpinRate.Selected = spinRateIdx;
		optionSpinRate.ItemSelected += OnOptionSpinRate;

		//--- CheckBox, Precession IC
		checkPrecess = vbox.GetNode<CheckBox>("CheckPrecess");
		checkPrecess.ButtonPressed = false;
		checkPrecess.Disabled = true;
		//checkPrecess.Set("theme_override_colors/font_color",new Color(1,0,0));
		checkPrecess.Pressed += OnCheckPrecess;

		//--- Option Button, Sim choice
		optionSim = vbox.GetNode<OptionButton>("OptionSim");
		optionSim.AddItem("Sim: Fixed Body",0);
		optionSim.AddItem("Sim: Lean Frame",1);
		optionSim.Selected = simOpt;
		optionSim.ItemSelected += OnOptionSim;

		//--- Sim Button
		simButton = vbox.GetNode<Button>("SimButton");
		simButton.Pressed += OnSimButtonPressed;

		//--- Adjustment buttons
		adjButtons = new Button[4];
		adjButtons[0] = 
			GetNode<Button>("UINode/MgContainTL/VBox/HBoxAdjust/LLeftButton");
		adjButtons[1] = 
			GetNode<Button>("UINode/MgContainTL/VBox/HBoxAdjust/LeftButton");
		adjButtons[1].Pressed += OnAdjButtonSlow;
		adjButtons[2] = 
			GetNode<Button>("UINode/MgContainTL/VBox/HBoxAdjust/RightButton");
		adjButtons[2].Pressed += OnAdjButtonSlow;
		adjButtons[3] = 
			GetNode<Button>("UINode/MgContainTL/VBox/HBoxAdjust/RRightButton");
		
		OnOptionSim((long)simOpt);
	}

	//------------------------------------------------------------------------
	// OnSimButtonPressed
	//------------------------------------------------------------------------
	private void OnSimButtonPressed()
	{
		int i;
		//GD.Print("OnSimButtonPressed");
		if(opMode == OpMode.Configure){
			simButton.Text = "Stop Sim";
			opMode = OpMode.Simulate;
			optionSim.Disabled = true;
			optionSimSubsteps.Disabled = true;
			optionSpinRate.Disabled = true;
			checkPrecess.Disabled = true;
			for(i=0;i<4;++i)
				adjButtons[i].Disabled = true;
		}
		else{
			simButton.Text = "Simulate";
			opMode = OpMode.Configure;
			optionSim.Disabled = false;
			optionSimSubsteps.Disabled = false;
			optionSpinRate.Disabled = false;
			if(simOpt == 1)
				checkPrecess.Disabled = false;
			for(i=0;i<4;++i)
				adjButtons[i].Disabled = false;
		}
	}

	//------------------------------------------------------------------------
    // OnAdjButtonSlow
    //------------------------------------------------------------------------
    private void OnAdjButtonSlow()
    {
        if(adjButtons[1].ButtonPressed){
            leanICDeg += dAngle;
            ProcessLeanAngle();
        }
        else if(adjButtons[2].ButtonPressed){
            leanICDeg -= dAngle;
            ProcessLeanAngle();
        }
    }

	//------------------------------------------------------------------------
    // OnOptionSimSubsteps
    //------------------------------------------------------------------------
    private void OnOptionSimSubsteps(long ii)
    {
		//GD.Print("Substeps: " + ii);
		if(ii == 0)
			nSimSteps = 1;
		if(ii == 1)
			nSimSteps = 2;
		if(ii == 2)
			nSimSteps = 4;
		if(ii == 3)
			nSimSteps = 8;
		if(ii == 4)
			nSimSteps = 16;
	}

	//------------------------------------------------------------------------
    // OnOptionSpinRate
    //------------------------------------------------------------------------
    private void OnOptionSpinRate(long ii)
    {
		spinRateIdx = (int)ii;
		spinRate = spinRateOptions[spinRateIdx];
		ProcessLeanAngle();

		//GD.Print("SpinRate: " + spinRate);
	}

	//------------------------------------------------------------------------
	// OnCheckPrecess
	//------------------------------------------------------------------------
	private void OnCheckPrecess()
	{
		//GD.Print("OnCheckPrecess");
		ProcessLeanAngle();
		if(checkPrecess.ButtonPressed == false){
			checkPrecess.Set("theme_override_colors/font_disabled_color",
					new Color(0.875f,0.875f,0.875f, 0.5f));
		}
	}

	//------------------------------------------------------------------------
    // OnOptionSim
    //------------------------------------------------------------------------
    private void OnOptionSim(long ii)
    {
		int idx = (int)ii;
		if(idx == 0){
			//GD.Print("Body Fixed");
			sim.SwitchModelBody();
			optionSpinRate.SetItemText(0,spinRateLabelOptions[0,0]);
			optionSpinRate.SetItemText(1,spinRateLabelOptions[0,1]);
			optionSpinRate.SetItemText(2,spinRateLabelOptions[0,2]);
			optionSpinRate.SetItemText(3,spinRateLabelOptions[0,3]);
			checkPrecess.ButtonPressed = false;
			checkPrecess.Disabled = true;
			checkPrecess.Set("theme_override_colors/font_disabled_color",
				new Color(0.875f,0.875f,0.875f, 0.5f));
			ProcessLeanAngle();
		}
		else if(idx == 1){
			//GD.Print("Lean Frame");
			sim.SwitchModelLean();
			optionSpinRate.SetItemText(0,spinRateLabelOptions[1,0]);
			optionSpinRate.SetItemText(1,spinRateLabelOptions[1,1]);
			optionSpinRate.SetItemText(2,spinRateLabelOptions[1,2]);
			optionSpinRate.SetItemText(3,spinRateLabelOptions[1,3]);
			checkPrecess.Disabled = false;
			ProcessLeanAngle();
		}
		simOpt = idx;
	}
}
