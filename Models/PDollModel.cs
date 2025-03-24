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

		
	}

	//------------------------------------------------------------------------
	// Initialize:
	//------------------------------------------------------------------------
	public void Initialize(float h, float L, float Lrod, float hUB, float hLB,
		float thk)
	{
		ubody.GenMesh(1.0f, hUB, 0.5f*thk);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
