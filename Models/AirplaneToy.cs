//============================================================================
// AirplaneToy.cs
//============================================================================
using Godot;
using System;

public partial class AirplaneToy : Node3D
{
	GeometryInstance3D geo;
	Node3D ConeX;
	Node3D ConeY;
	Node3D ConeZ;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		geo = GetNode<GeometryInstance3D>("11805AirplaneV2L2");
		ConeX = GetNode<Node3D>("ConeX");
		ConeY = GetNode<Node3D>("ConeY");
		ConeZ = GetNode<Node3D>("ConeZ");
	}

	public void SetTransparency(float val)
	{
		geo.Transparency = val;
	}

	public void ShowCones(bool b = true)
	{
		if(b){
			ConeX.Show();
			ConeY.Show();
			ConeZ.Show();
		}
		else{
			ConeX.Hide();
			ConeY.Hide();
			ConeZ.Hide();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
