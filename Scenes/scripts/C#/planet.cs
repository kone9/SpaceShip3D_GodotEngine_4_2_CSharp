using Godot;
using System;

public partial class planet : RigidBody3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector3 actualPosition = Position;

		if(Position.X < -3100)
		{
			actualPosition.X = 3100;
            Position = actualPosition;
			GD.Print("cambio de posiicon");
		}
	}
}
