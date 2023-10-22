using Godot;
using System;

public partial class enemy_spawner : Node3D
{

	[Export]
	PackedScene ASTEROID;

    [Export]
    PackedScene ENEMY;

    Godot.Collections.Array< Node > spawnpoints;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		spawnpoints = GetChildren();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	private void _on_timer_timeout_spawn_enemy()
	{
		var spawn_position = get_spawn_position_Aleatory();

        Node new_obstacle = null;

		if (GD.Randf() < 0.1) //se instancian pocos asteroides
		{
            new_obstacle = (Area3D)ASTEROID.Instantiate();
            (new_obstacle as Area3D).GlobalPosition = spawn_position;
        }
		else
		{
            new_obstacle = (RigidBody3D)ENEMY.Instantiate();
            (new_obstacle as RigidBody3D).GlobalPosition = spawn_position;
            (new_obstacle as RigidBody3D).RotationDegrees = new Vector3(0, -180, 0);

        }

        Node main = GetTree().CurrentScene;
        main.AddChild(new_obstacle);
		

    }




    private Vector3 get_spawn_position_Aleatory()
	{
		Godot.Collections.Array<Node> points = spawnpoints;
		points.Shuffle();

		return (points[0] as Marker3D).GlobalPosition;
	}


}
