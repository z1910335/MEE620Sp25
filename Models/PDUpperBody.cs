//============================================================================
// PDUpperBody.cs
// Immediate Mesh for the upper body.
//============================================================================
using Godot;
using System;
using System.Net;

public partial class PDUpperBody : MeshInstance3D
{
	float hWidth = 1.0f;
	float hThick = 0.25f;
	float height = 3.0f;

	ImmediateMesh mesh;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mesh = new ImmediateMesh();
	}

	public void GenMesh(float hw, float hgt, float hthk)
	{
		GD.Print("PDUpperBody:GenMesh");

		mesh.SurfaceBegin(Mesh.PrimitiveType.Triangles);

		// top face
		mesh.SurfaceSetNormal(new Vector3(0.0f, 1.0f, 0.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f, -hthk));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f,  hthk));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f,  hthk));

		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f, -hthk));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f, -hthk));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f,  hthk));

		// front face
		mesh.SurfaceSetNormal(new Vector3(0.0f, 0.0f, 1.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f, hthk));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f, hthk));
		mesh.SurfaceAddVertex(new Vector3( hw, -hgt, hthk));

		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f, hthk));
		mesh.SurfaceAddVertex(new Vector3( hw, -hgt, hthk));
		mesh.SurfaceAddVertex(new Vector3(-hw, -hgt, hthk));

		// left face
		mesh.SurfaceSetNormal(new Vector3(1.0f, 0.0f, 0.0f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f, hthk));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f,-hthk));
		mesh.SurfaceAddVertex(new Vector3( hw, -hgt,-hthk));

		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f, hthk));
		mesh.SurfaceAddVertex(new Vector3( hw, -hgt,-hthk));
		mesh.SurfaceAddVertex(new Vector3( hw, -hgt, hthk));

		// right face
		mesh.SurfaceSetNormal(new Vector3(-1.0f, 0.0f, 0.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f,-hthk));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f, hthk));
		mesh.SurfaceAddVertex(new Vector3(-hw, -hgt, hthk));

		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f,-hthk));
		mesh.SurfaceAddVertex(new Vector3(-hw, -hgt, hthk));
		mesh.SurfaceAddVertex(new Vector3(-hw, -hgt,-hthk));

		// back face
		mesh.SurfaceSetNormal(new Vector3(0.0f, 0.0f, -1.0f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f,-hthk));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f,-hthk));
		mesh.SurfaceAddVertex(new Vector3(-hw, -hgt,-hthk));

		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f,-hthk));
		mesh.SurfaceAddVertex(new Vector3(-hw, -hgt,-hthk));
		mesh.SurfaceAddVertex(new Vector3( hw, -hgt,-hthk));

		//mesh.SurfaceAddVertex(new Vector3(, , ));


		mesh.SurfaceEnd();

		Mesh = mesh;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
