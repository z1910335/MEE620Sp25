//============================================================================
// PDollModel.cs
// Model of the Playter Doll.
//============================================================================
using Godot;
using System;

public partial class PDollModel : Node3D
{
	bool initialized;

	Node3D rootNode;
	Node3D vertShift;
	PDUpperBody ubody;
	PdLowerBody lbody;
	Node3D shoulderFrame;

	Vector3 eulerAngles;

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		initialized = false;

		rootNode = GetNode<Node3D>("RootNode");
		vertShift = GetNode<Node3D>("RootNode/VertShift");
		ubody = GetNode<PDUpperBody>("RootNode/VertShift/PDUpperBody");
		lbody = GetNode<PdLowerBody>("RootNode/VertShift/PDLowerBody");
		shoulderFrame = vertShift.GetNode<Node3D>("ShoulderFrame");

		eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
	}

	//------------------------------------------------------------------------
	// Initialize:
	//------------------------------------------------------------------------
	public void Initialize(float h, float L, float Lrod, float hUB, float hLB,
		float thk, float shAngle)
	{
		float shoulderOffset = 0.5f*thk;

		ubody.GenMesh(1.0f, hUB, 0.5f*thk);
		vertShift.Position = new Vector3(0.0f, h+shoulderOffset, 0.0f);
		lbody.Position = new Vector3(0.0f, -hUB, 0.0f);
		lbody.GenMesh(1.0f, hLB, 0.5f*thk, 0.5f);
		
		shoulderFrame.Rotation = new Vector3(shAngle, 0.0f, 0.0f);
		shoulderFrame.Position = new Vector3(0.0f, -shoulderOffset, 0.0f);

		MeshInstance3D ShCylL = shoulderFrame.GetNode<MeshInstance3D>("CylL");
		ShCylL.Position = new Vector3(1.0f, 0.0f, 0.0f);
		ShCylL.Rotation = new Vector3(-0.5f*MathF.PI, 0.0f, 0.0f);
		CylinderMesh shMeshL = (CylinderMesh)ShCylL.Mesh;
		shMeshL.TopRadius = shoulderOffset;
		shMeshL.BottomRadius = shoulderOffset;
		shMeshL.Height = 1.2f*thk;

		MeshInstance3D ShCylR = shoulderFrame.GetNode<MeshInstance3D>("CylR");
		ShCylR.Position = new Vector3(-1.0f, 0.0f, 0.0f);
		ShCylR.Rotation = new Vector3(0.5f*MathF.PI, 0.0f, 0.0f);
		CylinderMesh shMeshR = (CylinderMesh)ShCylR.Mesh;
		shMeshR.TopRadius = shoulderOffset;
		shMeshR.BottomRadius = shoulderOffset;
		shMeshR.Height = 1.2f*thk;
	}


	//------------------------------------------------------------------------
	// UpdateSimpleRotation
	//------------------------------------------------------------------------
	public void SetSimpleRotation(float th)
	{
		eulerAngles.X = th;
		eulerAngles.Y = 0.0f;
		eulerAngles.Z = 0.0f;

		rootNode.Rotation = eulerAngles;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
