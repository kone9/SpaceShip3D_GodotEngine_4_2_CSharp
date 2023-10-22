using Godot;
using System;

public partial class enemy_spawner : Node3D
{

	[Export]
	PackedScene ASTEROID;

    [Export]
    PackedScene ENEMY;

    private Godot.Collections.Array< Node > spawnpoints;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		spawnpoints = GetTree().GetNodesInGroup("positionsSpawns");
	}

	//init timer in editor AutoStart
	private void _on_init_spawner_timer_timeout()
	{
		GetNode<Timer>("Timer_Spawn_Enemy").Start();
	}


    private void _on_timer_spawn_enemy_timeout()
	{
		if (GD.Randf() < 0.1) //Poca probabilidad que se instancien asteroides
		{
            InstanceEnemy(ASTEROID, get_spawn_position_Aleatory(), Vector3.Zero);
        }
		else
		{
            InstanceEnemy(ENEMY, get_spawn_position_Aleatory(), new Vector3(0, -180, 0));
        }
    }


	private void InstanceEnemy(PackedScene scene_obstacle, Vector3 spawn_position, Vector3 spawn_rotation)
	{
        Node3D new_obstacle = (Node3D)scene_obstacle.Instantiate();
        Node main = GetTree().CurrentScene;
        main.AddChild(new_obstacle);
        new_obstacle.GlobalPosition = spawn_position;
        new_obstacle.RotationDegrees = spawn_rotation;
    }


    private Vector3 get_spawn_position_Aleatory()
	{
		Godot.Collections.Array<Node> points = spawnpoints;
		points.Shuffle();
		return (points[0] as Marker3D).GlobalPosition;
	}


}
