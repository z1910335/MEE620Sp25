//============================================================================
// StickPuck.cs  Script for customizing the model 
//============================================================================
using Godot;
using System;

public partial class StickPuck : Node3D
{
	MeshInstance3D stick;
	BoxMesh stickMesh;
	MeshInstance3D puck;
	CylinderMesh puckMesh;

	float stickLength;  // length of stick
	float stickThick;    // diameter of stick
	float puckDiam;     // diameter of ball
	float puckOverHang; // how much wider puck is (on each side of stick)

	//------------------------------------------------------------------------
	// _Ready: called once
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		stickLength = 1.0f;
		stickThick = 0.06f;
		puckDiam = 0.3f;
		puckOverHang = 0.01f;

		stick = GetNode<MeshInstance3D>("Stick");
		stickMesh = (BoxMesh)stick.Mesh;

		puck = GetNode<MeshInstance3D>("Puck");
		puckMesh = (CylinderMesh)puck.Mesh;
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
				stickMesh.Size =  
					new Vector3(stickThick,stickLength,stickThick);
				puck.Position = new Vector3(0.0f, -stickLength, 0.0f);
			}
		}

		get{
			return stickLength;
		}
	}

	public float StickThickness
	{
		set{
			if(value >= 0.001){
				stickThick = value;
				stickMesh.Size =  
					new Vector3(stickThick,stickLength,stickThick);
				puckMesh.Height = stickThick + 2.0f*puckOverHang;
			}
		}

		get{
			return stickThick;
		}
	}

	public float PuckDiameter
	{
		set{
			if(value > 0.001){
				puckDiam = value;
				puckMesh.TopRadius = 0.5f * puckDiam;
				puckMesh.BottomRadius = 0.5f * puckDiam;
			}
		}

		get{
			return puckDiam;
		}
	}
}
