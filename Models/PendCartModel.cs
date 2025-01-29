//============================================================================
// StickPuck.cs  Script for customizing the model 
//============================================================================
using Godot;
using System;

public partial class PendCartModel : Node3D
{
	Node3D rootNode;
	StickPuck pendModel;

	Node3D[] Wheels;

	Vector3 boxSize;
	float pendLength;
	float wheelRad;
	float wheelThick;
	float pinOverhang;    // not used yet

	Vector3 cartLoc;    // generalized coordinate, cart location
	Vector3 pendAngle;  // generalized coord: pendulum angle 
	Vector3 wheelAngle; 

	//------------------------------------------------------------------------
	// _Ready: called once
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		boxSize = new Vector3(0.5f, 0.35f, 0.5f);
		pendLength = 0.8f;
		wheelRad = 0.1f;
		wheelThick = 0.05f;

		rootNode = GetNode<Node3D>("RootNode");

		pendModel = GetNode<StickPuck>("RootNode/Box/StickPuck");

		Wheels = new Node3D[4];
		Wheels[0] = GetNode<Node3D>("RootNode/WheelNode1");
		Wheels[1] = GetNode<Node3D>("RootNode/WheelNode2");
		Wheels[2] = GetNode<Node3D>("RootNode/WheelNode3");
		Wheels[3] = GetNode<Node3D>("RootNode/WheelNode4");

		cartLoc = new Vector3();   // initializes with zeros
		pendAngle = new Vector3();
		wheelAngle = new Vector3();

		SetParams();
	}

	//------------------------------------------------------------------------
	// SetPositionAngle: sets the cart location and pendulum angle
	//------------------------------------------------------------------------
	public void SetPositionAngle(float x, float theta)
	{
		cartLoc.X = x;
		pendAngle.Z = theta;
		wheelAngle.Z = -x/wheelRad;

		rootNode.Position = cartLoc;
		pendModel.Rotation = pendAngle;
		for(int i=0;i<4;++i){
			Wheels[i].Rotation = wheelAngle;
		}
	}

	//------------------------------------------------------------------------
	// SetParams: Sets size parameters of the model
	//------------------------------------------------------------------------
	private void SetParams()
	{
		cartLoc.Z = -0.5f*boxSize.Z - 2.5f*wheelThick;
		rootNode.Position = cartLoc;

		MeshInstance3D box = GetNode<MeshInstance3D>("RootNode/Box");
		BoxMesh boxMesh = (BoxMesh)box.Mesh;
		boxMesh.Size = boxSize;
		box.Position = new Vector3(0.0f, 0.5f*boxSize.Y + wheelRad, 0.0f);

		pendModel.Length = pendLength;

		Wheels[0].Position = new Vector3(-0.5f*boxSize.X + 0.5f*wheelRad,
			wheelRad, 0.5f*boxSize.Z + wheelThick);
		Wheels[1].Position = new Vector3(0.5f*boxSize.X - 0.5f*wheelRad,
			wheelRad, 0.5f*boxSize.Z + wheelThick);
		Wheels[2].Position = new Vector3(-0.5f*boxSize.X + 0.5f*wheelRad,
			wheelRad, -0.5f*boxSize.Z - wheelThick);
		Wheels[3].Position = new Vector3(0.5f*boxSize.X - 0.5f*wheelRad,
			wheelRad, -0.5f*boxSize.Z - wheelThick);

		int i;
		for(i=0;i<4;++i){
			CylinderMesh cmesh = (CylinderMesh)Wheels[i].
				GetNode<MeshInstance3D>("Wheel").Mesh;
			cmesh.TopRadius = wheelRad;
			cmesh.BottomRadius = wheelRad;
			cmesh.Height = wheelThick;
		}
		
	}

	//------------------------------------------------------------------------
	// Setters
	//------------------------------------------------------------------------
	public float CartLength
	{
		set{
			if(value >= 0.05){
				boxSize.X = value;
				SetParams();
			}
		}
	}

	public float WheelRadius
	{
		set{
			if(value >= 0.01){
				wheelRad = value;
				SetParams();
			}
		}
	}

	public float WheelThickness
	{
		set{
			if(value >= 0.001){
				wheelThick = value;
				SetParams();
			}
		}
	}

	public float PendulumLength
	{
		set{
			if(value >= 0.2)
			{
				pendLength = value;
				SetParams();
			}
		}
	}
}
