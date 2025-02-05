//============================================================================
// GimbalToy.cs
// STUDENTS SHOULD NOT MODIFY THIS FILE
//============================================================================
using Godot;
using System;

public partial class GimbalToy : Node3D
{
	Node3D outerRingNode;
	Node3D middleRingNode;
	Node3D innerRingNode;
	Node3D refModel; // the reference model
	bool hasRefModel; // whether the reference model has been provided

	Vector3 rot1;    // first rotation vector
	Vector3 rot2;    // second rotation vector
	Vector3 rot3;    // third rotation vector

	Basis basisCalc;
	Vector3 basisX;  // basis vectors
	Vector3 basisY;
	Vector3 basisZ;
	Quaternion ghostQuat;

	GimbalDCM calcDCM;

	enum EulerAngleMode{
		None,
		YPR,
		PYR,
		RYP,
		RPY,
	}

	EulerAngleMode eulerMode;

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		eulerMode = EulerAngleMode.None;

		outerRingNode = GetNode<Node3D>("OuterRingNode");
		middleRingNode = GetNode<Node3D>("OuterRingNode/MiddleRingNode");
		innerRingNode = GetNode<Node3D>
			("OuterRingNode/MiddleRingNode/InnerRingNode");

		hasRefModel = false;

		rot1 = new Vector3();
		rot2 = new Vector3();
		rot3 = new Vector3();

		calcDCM = new GimbalDCM();
		basisX = new Vector3();
		basisY = new Vector3();
		basisZ = new Vector3();
		basisCalc = new Basis();
		ghostQuat = new Quaternion();
	}

	//------------------------------------------------------------------------
	// SetRefModel:  Provides the reference model
	//------------------------------------------------------------------------
	public void SetRefModel(Node3D refm)
	{
		refModel = refm;
		hasRefModel = true;
	}

	//------------------------------------------------------------------------
	// Setup: Takes the requested gimbal configuration and calls the 
	//        appropriate setup routine
	//------------------------------------------------------------------------
	public void Setup(string mm)
	{
		string modeString = mm.ToUpper();

		if(modeString == "YPR")
			SetupYPR();
		else if(modeString == "RYP")
			SetupRYP();
	}

	//------------------------------------------------------------------------
	// SetAngles:
	//------------------------------------------------------------------------
	public int SetAngles(float angle1, float angle2, float angle3)
	{
		int retCode = 0;

		if(eulerMode == EulerAngleMode.YPR)
			SetAnglesYPR(angle1, angle2, angle3);
		else if(eulerMode == EulerAngleMode.RYP)
			SetAnglesRYP(angle1, angle2, angle3);
		else{
			GD.PrintErr("ApplyAngles -- Something's wrong.");
			return 1;
		}

		outerRingNode.Rotation = rot1;
		middleRingNode.Rotation = rot2;
		innerRingNode.Rotation = rot3;

		//GD.Print("Process the DCM");
		//process the DCM
		bool dcmChecks = true;
		float det = basisX.X * basisY.Y * basisZ.Z +
			basisX.Z * basisY.X * basisZ.Y +
			basisX.Y * basisY.Z * basisZ.X -

			basisX.Z * basisY.Y * basisZ.X -
			basisX.Y * basisY.X * basisZ.Z -
			basisX.X * basisY.Z * basisZ.Y;

		if(Mathf.Abs(det -1.0f) > 0.002f)
			dcmChecks = false;

		if(Mathf.Abs(basisX.Dot(basisY)) > 0.002f )
			dcmChecks = false;

		if(Mathf.Abs(basisX.Dot(basisZ)) > 0.002f )
			dcmChecks = false;

		if(Mathf.Abs(basisZ.Dot(basisY)) > 0.002f )
			dcmChecks = false;

		// GD.Print("--- " + basisY.Dot(basisZ));
		// GD.Print(basisX);
		// GD.Print(basisY);
		// GD.Print(basisZ);

		if(dcmChecks){
			basisCalc.X = basisX;
			basisCalc.Y = basisY;
			basisCalc.Z = basisZ;

			ghostQuat = basisCalc.GetRotationQuaternion();
			refModel.Quaternion = ghostQuat;
		}
		else{
			//GD.PrintErr("Bad DCM");
			retCode = 2;
		}

		return retCode;
	}

	//------------------------------------------------------------------------
	// SetupYPR: Yaw Pich Roll gimbal configuration
	//------------------------------------------------------------------------
	private void SetupYPR()
	{
		eulerMode = EulerAngleMode.YPR;
		
		ResetRings();

		Node3D outerRing = GetNode<Node3D>("OuterRingNode/OuterRing");
		outerRing.Rotation = new Vector3(0.5f*Mathf.Pi, 0.5f*Mathf.Pi, 0.0f);

		Node3D onn = GetNode<Node3D>("OuterRingNode/OuterNubNode");
		onn.Rotation = new Vector3(0.0f, 0.0f, 0.5f*Mathf.Pi);
	}
	
	//------------------------------------------------------------------------
	// SetAnglesYPR: Yaw Pitch Roll Euler angle application
	//------------------------------------------------------------------------
	private void SetAnglesYPR(float angle1, float angle2, float angle3)
	{
		rot1.Y = angle1;
		rot2.Z = angle2;
		rot3.X = angle3;

		calcDCM.CalcDCM_YPR(angle1, angle2, angle3);
		basisX.X = calcDCM.GetDCM(0,0);
		basisX.Y = calcDCM.GetDCM(1,0);
		basisX.Z = calcDCM.GetDCM(2,0);

		basisY.X = calcDCM.GetDCM(0,1);
		basisY.Y = calcDCM.GetDCM(1,1);
		basisY.Z = calcDCM.GetDCM(2,1);

		basisZ.X = calcDCM.GetDCM(0,2);
		basisZ.Y = calcDCM.GetDCM(1,2);
		basisZ.Z = calcDCM.GetDCM(2,2);
	}

	//------------------------------------------------------------------------
	// SetupRYP: Roll Yaw Pitchgimbal configuration
	//------------------------------------------------------------------------
	private void SetupRYP()
	{
		eulerMode = EulerAngleMode.RYP;
		
		ResetRings();

		Node3D outerRing = GetNode<Node3D>("OuterRingNode/OuterRing");
		outerRing.Rotation = new Vector3(0.0f , 0.5f*Mathf.Pi, 0.5f*Mathf.Pi);

		Node3D middleRing = GetNode<Node3D>(
			"OuterRingNode/MiddleRingNode/MiddleRing");
		middleRing.Rotation = new Vector3(0.5f*Mathf.Pi, 0.5f*Mathf.Pi, 0.0f);

		//Node3D outerRing = GetNode<Node3D>("OuterRingNode/OuterRing");
		//outerRing.Rotation = new Vector3(0.5f*Mathf.Pi, 0.5f*Mathf.Pi, 0.0f);

		//Node3D onn = GetNode<Node3D>("OuterRingNode/OuterNubNode");
		//onn.Rotation = new Vector3(0.0f, 0.0f, 0.5f*Mathf.Pi);
	}

	//------------------------------------------------------------------------
	// SetAnglesYPR: Yaw Pitch Roll Euler angle application
	//------------------------------------------------------------------------
	private void SetAnglesRYP(float angle1, float angle2, float angle3)
	{
		rot1.X = angle1;
		rot2.Y = angle2;
		rot3.Z = angle3;

		calcDCM.CalcDCM_RYP(angle1, angle2, angle3);
		basisX.X = calcDCM.GetDCM(0,0);
		basisX.Y = calcDCM.GetDCM(1,0);
		basisX.Z = calcDCM.GetDCM(2,0);

		basisY.X = calcDCM.GetDCM(0,1);
		basisY.Y = calcDCM.GetDCM(1,1);
		basisY.Z = calcDCM.GetDCM(2,1);

		basisZ.X = calcDCM.GetDCM(0,2);
		basisZ.Y = calcDCM.GetDCM(1,2);
		basisZ.Z = calcDCM.GetDCM(2,2);
	}

	
	//------------------------------------------------------------------------
	// ResetRings
	//------------------------------------------------------------------------
	private void ResetRings()
	{
		Node3D onn = GetNode<Node3D>("OuterRingNode/OuterNubNode");
		onn.Rotation = new Vector3(0.0f, 0.0f, 0.0f);

		Node3D outerRing = GetNode<Node3D>("OuterRingNode/OuterRing");
		outerRing.Rotation = new Vector3(0.0f, 0.0f, 0.0f);

		Node3D middleRing = GetNode<Node3D>(
			"OuterRingNode/MiddleRingNode/MiddleRing");
		middleRing.Rotation = new Vector3(0.0f, 0.0f, 0.0f);
	}
}
