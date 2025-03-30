//============================================================================
// PDLowerBody.cs
// Immediate Mesh for the lower body.
//============================================================================
using Godot;
using System;

public partial class PdLowerBody : MeshInstance3D
{

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
	// hw: half width
	// hgt: height
	// hthk: half thickness
	// tprFrac: taperfraction
	//------------------------------------------------------------------------
	public void GenMesh(float hw, float hgt, float hthk, float tprFrac)
	{
		//GD.Print("PdLowerBody:GenMesh");

		float hwb = hw*tprFrac;  // Half width at the bottom

		float ovl = hw-hwb;  // overlap
		float hyp = Mathf.Sqrt(ovl*ovl + hgt*hgt);

		Vector3 normLt = new Vector3(hgt/hyp, -ovl/hyp, 0.0f);
		Vector3 normRt = new Vector3(-hgt/hyp, -ovl/hyp, 0.0f);

		mesh.SurfaceBegin(Mesh.PrimitiveType.Triangles);

		// front face
		mesh.SurfaceSetNormal(new Vector3(0.0f, 0.0f, 1.0f));
		mesh.SurfaceSetUV(new Vector2(0.33f,0.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f, hthk));
		mesh.SurfaceSetUV(new Vector2(0.67f,0.0f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f, hthk));
		mesh.SurfaceSetUV(new Vector2(0.67f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hwb, -hgt, hthk));

		mesh.SurfaceSetUV(new Vector2(0.33f,0.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f, hthk));
		mesh.SurfaceSetUV(new Vector2(0.67f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hwb, -hgt, hthk));
		mesh.SurfaceSetUV(new Vector2(0.33f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hwb, -hgt, hthk));

		// left face
		mesh.SurfaceSetNormal(normLt);
		mesh.SurfaceSetUV(new Vector2(0.67f,0.0f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f, hthk));
		mesh.SurfaceSetUV(new Vector2(1.0f,0.0f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f,-hthk));
		mesh.SurfaceSetUV(new Vector2(1.0f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hwb, -hgt,-hthk));

		mesh.SurfaceSetUV(new Vector2(0.67f,0.0f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f, hthk));
		mesh.SurfaceSetUV(new Vector2(1.0f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hwb, -hgt,-hthk));
		mesh.SurfaceSetUV(new Vector2(0.67f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hwb, -hgt, hthk));

		// right face
		mesh.SurfaceSetNormal(normRt);
		mesh.SurfaceSetUV(new Vector2(0.0f,0.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f,-hthk));
		mesh.SurfaceSetUV(new Vector2(0.33f,0.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f, hthk));
		mesh.SurfaceSetUV(new Vector2(0.33f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hwb, -hgt, hthk));

		mesh.SurfaceSetUV(new Vector2(0.33f,0.0f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f,-hthk));
		mesh.SurfaceSetUV(new Vector2(0.0f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hwb, -hgt, hthk));
		mesh.SurfaceSetUV(new Vector2(0.33f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hwb, -hgt,-hthk));

		// back face
		mesh.SurfaceSetNormal(new Vector3(0.0f, 0.0f, -1.0f));
		mesh.SurfaceSetUV(new Vector2(0.0f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f,-hthk));  //tl
		mesh.SurfaceSetUV(new Vector2(0.33f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hw, 0.0f,-hthk));  //tr
		mesh.SurfaceSetUV(new Vector2(0.3f,1.0f));
		mesh.SurfaceAddVertex(new Vector3(-hwb, -hgt,-hthk));  //br

		mesh.SurfaceSetUV(new Vector2(0.0f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hw, 0.0f,-hthk));  //tl
		mesh.SurfaceSetUV(new Vector2(0.33f,1.0f));
		mesh.SurfaceAddVertex(new Vector3(-hwb, -hgt,-hthk));  //br
		mesh.SurfaceSetUV(new Vector2(0.0f,1.0f));
		mesh.SurfaceAddVertex(new Vector3( hwb, -hgt,-hthk));  //bl

		// bottom face
		mesh.SurfaceSetNormal(new Vector3(0.0f, -1.0f, 0.0f));
		mesh.SurfaceSetUV(new Vector2(0.67f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hwb, -hgt,hthk));
		mesh.SurfaceSetUV(new Vector2(1.0f,0.5f));
		mesh.SurfaceAddVertex(new Vector3( hwb, -hgt,hthk));
		mesh.SurfaceSetUV(new Vector2(1.0f,1.0f));
		mesh.SurfaceAddVertex(new Vector3( hwb, -hgt,-hthk));

		mesh.SurfaceSetUV(new Vector2(0.67f,0.5f));
		mesh.SurfaceAddVertex(new Vector3(-hwb, -hgt,hthk));
		mesh.SurfaceSetUV(new Vector2(1.0f,1.0f));
		mesh.SurfaceAddVertex(new Vector3( hwb, -hgt,-hthk));
		mesh.SurfaceSetUV(new Vector2(0.67f,1.0f));
		mesh.SurfaceAddVertex(new Vector3( -hwb, -hgt,-hthk));

		mesh.SurfaceEnd();

		Mesh = mesh;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
