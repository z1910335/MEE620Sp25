//============================================================================
// StickBall.cs  Script for customizing the model 
//============================================================================
using Godot;
using System;

public partial class StickBall : Node3D
{
	MeshInstance3D stick;
	CylinderMesh stickMesh;
	MeshInstance3D ball;
	SphereMesh ballMesh;

	float stickLength;  // length of stick
	float stickDiam;    // diameter of stick
	float ballDiam;     // diameter of ball


	//------------------------------------------------------------------------
	// _Ready: called once
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		stickLength = 1.0f;
		stickDiam = 0.06f;
		ballDiam = 0.3f;

		stick = GetNode<MeshInstance3D>("Stick");
		stickMesh = (CylinderMesh)stick.Mesh;

		ball = GetNode<MeshInstance3D>("Ball");
		ballMesh = (SphereMesh)ball.Mesh;
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
	}

	//------------------------------------------------------------------------
	// setters
	//------------------------------------------------------------------------
	public float Length
	{
		set{
			if(value > 0.05){
				stickLength = value;
				stick.Position = new Vector3(0.0f, -0.5f*stickLength, 0.0f);
				stickMesh.Height = stickLength;
				ball.Position = new Vector3(0.0f, -stickLength, 0.0f);
			}
		}

		get{
			return stickLength;
		}
	}

	public float StickDiameter
	{
		set{
			if(value >= 0.001){
				stickDiam = value;
				stickMesh.TopRadius = 0.5f * stickDiam;
				stickMesh.BottomRadius = 0.5f * stickDiam;
			}
		}

		get{
			return stickDiam;
		}
	}

	public float BallDiameter
	{
		set{
			if(value > 0.001){
				ballDiam = value;
				ballMesh.Radius = 0.5f * ballDiam;
				ballMesh.Height = ballDiam;
			}
		}

		get{
			return ballDiam;
		}
	}
}
