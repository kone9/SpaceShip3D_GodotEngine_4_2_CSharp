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




	public void _on_area_3d_area_entered(Area3D area) 
	{
        if (area.IsInGroup("meteor") || area.IsInGroup("enemyColision"))
		{
			QueueFree();
		}
    }


    public async void _on_visible_on_screen_notifier_3d_screen_exited()
	{
        await ToSignal(GetTree().CreateTimer(1f), "timeout");//detengo por 0.2 por la explosion
		QueueFree();
    }

}
