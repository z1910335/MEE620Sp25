//============================================================================
// StickPuck.cs  Script for customizing the model 
//============================================================================
using Godot;
using System;
using System.IO.Pipes;

public partial class TopDiskModel : Node3D
{
	Node3D PrecessNode;
	Node3D LeanNode;
	Node3D SpinNode;

	Vector3 precessAngle;
	Vector3 leanAngle;
	Vector3 spinAngle;
	
	//------------------------------------------------------------------------
	// _Ready: called once
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		
		PrecessNode = GetNode<Node3D>("PrecessNode");
		LeanNode = GetNode<Node3D>("PrecessNode/LeanNode");
		SpinNode = GetNode<Node3D>("PrecessNode/LeanNode/SpinNode");

		precessAngle = new Vector3();
		leanAngle = new Vector3();
		spinAngle = new Vector3();
	}

	
	//------------------------------------------------------------------------
	// SetEulerAnglesYZY:
	//------------------------------------------------------------------------
	public void SetEulerAnglesYZY(float psi, float phi, float theta)
	{
		precessAngle.Y = psi;
		leanAngle.Z = phi;
		spinAngle.Y = theta;

		PrecessNode.Rotation = precessAngle;
		LeanNode.Rotation = leanAngle;
		SpinNode.Rotation = spinAngle;
	}

	// public override void _Process(double delta)
	// {
	// }
}
