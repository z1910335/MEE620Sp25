//============================================================================
// PlayterDoll.cs
// Code for runing the Godot scene of rotating playter doll
//============================================================================
using Godot;
using System;

public partial class PlayterDoll : Node3D
{
	float cgHeight;

	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target


	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("PlayterDoll");

		cgHeight = 3.0f;

		// Set up the camera rig
		longitudeDeg = 30.0f;
		latitudeDeg = 15.0f;
		camDist = 4.0f;

		camTg = new Vector3(0.0f, cgHeight, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.Target = camTg;
		cam.Position = new Vector3(0.0f, cgHeight, 0.0f);
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
	}
}
