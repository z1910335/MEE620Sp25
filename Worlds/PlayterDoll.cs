//============================================================================
// PlayterDoll.cs
// Code for runing the Godot scene of rotating playter doll
//============================================================================
using Godot;
using System;

public partial class PlayterDoll : Node3D
{
	// parameters
	double ma = 0.107;     // arm mass ratio
	double rho = 2.21;     // dimensionless radius of gyration
	double gammay = 0.091; // ratio I_Gy/I_Gx
	double gammaz = 1.05;  // ratio I_Gz/I_Gx
	double h = 1.56;       // dimensionless height of shoulder above CG
	double L = 1.65;       // dimensionless length to arm mass
	double Lrod = 2.5;    // length of arm rod (for visual)
	double phi = 0.0;      // shoulder angle

	float cgHeight;

	// model
	PDollModel model;

	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	double theta;   // somersault angle for testing

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("PlayterDoll");

		cgHeight = 6.0f;

		// set up doll model
		model = GetNode<PDollModel>("PDollModel");
		model.Position = new Vector3(0.0f, cgHeight, 0.0f);
		model.Initialize((float)h, (float)L, (float)Lrod, 3.0f /*hUb*/, 
		3.75f /*hLb*/, 0.5f /*thk*/, (float)phi);

		// Set up the camera rig
		longitudeDeg = 30.0f;  //30
		latitudeDeg = 15.0f;
		camDist = 12.0f;

		camTg = new Vector3(0.0f, cgHeight, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.Target = camTg;
		cam.Position = new Vector3(0.0f, cgHeight, 0.0f);

		theta = 0.0;
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
		model.SetSimpleRotation((float)theta);
	}

	//------------------------------------------------------------------------
	// _PhysicsProcess:
	//------------------------------------------------------------------------
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		theta += delta;
    }
}
