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

		eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
	}

	//------------------------------------------------------------------------
	// Initialize:
	//------------------------------------------------------------------------
	public void Initialize(float h, float L, float Lrod, float hUB, float hLB,
		float thk)
	{
		ubody.GenMesh(1.0f, hUB, 0.5f*thk);
		vertShift.Position = new Vector3(0.0f, h, 0.0f);
		lbody.Position = new Vector3(0.0f, -hUB, 0.0f);
		lbody.GenMesh(1.0f, hLB, 0.5f*thk, 0.5f);
		
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
