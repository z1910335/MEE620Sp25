//============================================================================
// PDUpperBody.cs
// Immediate Mesh for the upper body.
//============================================================================
using Godot;
using System;

public partial class PDUpperBody : MeshInstance3D
{
	// float hWidth = 1.0f;
	// float hThick = 0.25f;
	// float height = 3.0f;

	ImmediateMesh mesh;
	
	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		mesh = new ImmediateMesh();
	}

	//------------------------------------------------------------------------
	// GenMesh: generate mesh
	//------------------------------------------------------------------------
	public void GenMesh(float hw, float hgt, float hthk)
	{
		//GD.Print("PDUpperBody:GenMesh");

		mesh.SurfaceBegin(Mesh.PrimitiveType.Triangles);

		// top face
		mesh.SurfaceSetNormal(new Vector3(0.0f, 1.0f, 0.0f));
		mesh.SurfaceSetUV(new Vector2(0.33f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f, -hthk));
		mesh.SurfaceSetUV(new Vector2(0.67f,1.0f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f,  hthk));
		mesh.SurfaceSetUV(new Vector2(0.33f, 1.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f,  hthk));

		mesh.SurfaceSetUV(new Vector2(0.33f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f, -hthk));
		mesh.SurfaceSetUV(new Vector2(0.67f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f, -hthk));
		mesh.SurfaceSetUV(new Vector2(0.67f, 1.0f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f,  hthk));

		// front face
		mesh.SurfaceSetNormal(new Vector3(0.0f, 0.0f, 1.0f));
		mesh.SurfaceSetUV(new Vector2(0.33f,0.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f, hthk));
		mesh.SurfaceSetUV(new Vector2(0.67f,0.0f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f, hthk));
		mesh.SurfaceSetUV(new Vector2(0.67f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hw, -hgt, hthk));

		mesh.SurfaceSetUV(new Vector2(0.33f,0.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f, hthk));
		mesh.SurfaceSetUV(new Vector2(0.67f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hw, -hgt, hthk));
		mesh.SurfaceSetUV(new Vector2(0.33f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hw, -hgt, hthk));

		// left face
		mesh.SurfaceSetNormal(new Vector3(1.0f, 0.0f, 0.0f));
		mesh.SurfaceSetUV(new Vector2(0.67f,0.0f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f, hthk));
		mesh.SurfaceSetUV(new Vector2(1.0f,0.0f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f,-hthk));
		mesh.SurfaceSetUV(new Vector2(1.0f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hw, -hgt,-hthk));

		mesh.SurfaceSetUV(new Vector2(0.67f,0.0f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f, hthk));
		mesh.SurfaceSetUV(new Vector2(1.0f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hw, -hgt,-hthk));
		mesh.SurfaceSetUV(new Vector2(0.67f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hw, -hgt, hthk));

		// right face
		mesh.SurfaceSetNormal(new Vector3(-1.0f, 0.0f, 0.0f));
		mesh.SurfaceSetUV(new Vector2(0.0f,0.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f,-hthk));
		mesh.SurfaceSetUV(new Vector2(0.33f,0.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f, hthk));
		mesh.SurfaceSetUV(new Vector2(0.33f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hw, -hgt, hthk));

		mesh.SurfaceSetUV(new Vector2(0.33f,0.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f,-hthk));
		mesh.SurfaceSetUV(new Vector2(0.0f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hw, -hgt, hthk));
		mesh.SurfaceSetUV(new Vector2(0.33f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hw, -hgt,-hthk));

		// back face
		mesh.SurfaceSetNormal(new Vector3(0.0f, 0.0f, -1.0f));
		mesh.SurfaceSetUV(new Vector2(0.0f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f,-hthk));  //tl
		mesh.SurfaceSetUV(new Vector2(0.33f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f,-hthk));  //tr
		mesh.SurfaceSetUV(new Vector2(0.3f,1.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, -hgt,-hthk));  //br

		mesh.SurfaceSetUV(new Vector2(0.0f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f,-hthk));  //tl
		mesh.SurfaceSetUV(new Vector2(0.33f,1.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, -hgt,-hthk));  //br
		mesh.SurfaceSetUV(new Vector2(0.0f,1.0f));
		mesh.SurfaceAddVertex(new Vector3( hw, -hgt,-hthk));  //bl

		mesh.SurfaceEnd();

		Mesh = mesh;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
