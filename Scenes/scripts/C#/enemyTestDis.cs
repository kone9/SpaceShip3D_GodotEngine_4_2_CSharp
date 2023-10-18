using Godot;
using System;

public partial class enemyTestDis : RigidBody3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_area_3d_area_entered(Area3D area)
	{
		//if (area == null) return;

		if(area.IsInGroup("player"))
		{
			GD.Print("hola mundo desde C#");
            (GetTree().GetFirstNodeInGroup("explosionSound") as AudioStreamPlayer).Play();
            (GetTree().GetFirstNodeInGroup("hitSound") as AudioStreamPlayer).Play();
            QueueFree(); 
		}
		if(area.IsInGroup("ball"))
		{
			GD.Print("colisiono con la bala");
            (GetTree().GetFirstNodeInGroup("explosionSound") as AudioStreamPlayer).Play();
            (GetTree().GetFirstNodeInGroup("hitSound") as AudioStreamPlayer).Play();
            area.GetParent().QueueFree();
			QueueFree();
        }
	}

}
