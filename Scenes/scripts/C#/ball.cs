using Godot;
using System;

public partial class ball : RigidBody3D
{
 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GD.Randomize();

        if (GD.Randi() % 2 == 1)
        {
            (GetTree().GetFirstNodeInGroup("shoot") as AudioStreamPlayer).Play();
        }
		else
		{
            (GetTree().GetFirstNodeInGroup("shootdos") as AudioStreamPlayer).Play();
        }
	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	private void _on_area_3d_area_entered(Area3D area) 
	{


	}

}
