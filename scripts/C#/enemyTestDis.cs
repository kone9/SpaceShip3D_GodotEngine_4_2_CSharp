using Godot;
using System;
using System.Threading.Tasks;

public partial class enemyTestDis : RigidBody3D
{

	bool enemyDead = false;

    public bool EnemyDead { get => enemyDead; set => enemyDead = value; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public async void _on_area_3d_area_entered(Area3D area)
	{
		//if (area == null) return;
		if (EnemyDead == true) return;

        if (area.IsInGroup("meteor"))
        {
           await DestroyEnemy(area);
        }

        if (area.IsInGroup("player"))
		{
            await DestroyEnemy(area);
        }

		if(area.IsInGroup("ball"))
        {
            area.GetParent().QueueFree();//la bala tengo que destruirla
            await DestroyEnemy(area);
        }
    }

    public async void _on_visible_on_screen_notifier_3d_screen_exited()
    {
        await ToSignal(GetTree().CreateTimer(1f), "timeout");//detengo por 0.2 por la explosion
        QueueFree();
    }




    private async Task DestroyEnemy(Area3D area)
    {
        EnemyDead = true;

        GetNode<GpuParticles3D>("GPUParticles3Dfire").Emitting = true;
        GetNode<MeshInstance3D>("enemy3").Visible = false;
        (GetTree().GetFirstNodeInGroup("explosionSound") as AudioStreamPlayer).Play();
        (GetTree().GetFirstNodeInGroup("hitSound") as AudioStreamPlayer).Play();

        //GetNode<Area3D>("enemyColision").Monitorable = false;
        //GetNode<Area3D>("enemyColision").Monitoring = false;
        // GetNode<Area3D>("enemyColision").Visible = false;
        //GetNode<CollisionShape3D>("enemyColision/CollisionShapeAreaEnemY").Disabled = true;

        await ToSignal(GetTree().CreateTimer(0.2f), "timeout");//detengo por 0.2 por la explosion
        GetNode<Area3D>("enemycolision").QueueFree();

        await ToSignal(GetTree().CreateTimer(2f), "timeout");//detengo por 1 segundo

        QueueFree();
    }
}
