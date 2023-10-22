using Godot;
using System;
using System.Threading.Tasks;

public partial class player : RigidBody3D
{
    [Export]
    private float VELOCITY_MOVEMENT = 9f;
    [Export]
    private float VELOCITY_DESACELERATION = 5f;
    [Export]
    private Vector3 POSITION_RELOAD = new Vector3(-30, 0, 0);
    [Export]
    private int POSITION_FINAL_RELOAD_X = -30;
    [Export]
    private float VELOCITY_RELOAD = 30f;
    [Export]
    private PackedScene BALL;
    [Export]
    private PackedScene FIRE_EXPLOSION;

    public Vector2 velocity = Vector2.Zero;
    private bool CanDesacelerate = false;
    private AnimationTree animations;
    
    private Node3D main;
    private Node3D positionShoot;

    private bool playerDead = false;
    private bool canReposition  = false;

    public bool PlayerDead { get => playerDead; set => playerDead = value; }

    public override void _Ready()
	{
        animations = GetNode<AnimationTree>("naveRig/AnimationTreeNave");
        positionShoot = GetNode<Node3D>("positionShoot");
        main = GetTree().GetFirstNodeInGroup("main") as Node3D;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        Input_Exit_Game();
        RespawnPosition(delta);
        Input_Shoot(); 

    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
       
        InputAccelerateRigidBody();
    }

    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        base._IntegrateForces(state);
        DesacelerateRigidBody(state);
    }


    public async void _on_area_3d_player_area_entered(Area3D area)
    {
        if (area.IsInGroup("enemyColision"))
        {
            await Desactive_player(area);
        }
    }



    //--- Input Exit Game ---//

    private void Input_Exit_Game()
    {
        if (Input.IsActionJustPressed("escape"))
        {
            GetTree().Quit();
        }
    }


    //--- Input shoot ---//

    private void Input_Shoot()
    {
        if (PlayerDead == true) return;

        if (Input.IsActionJustPressed("space"))
        {
            RigidBody3D newShoot = (RigidBody3D)BALL.Instantiate();
            main.AddChild(newShoot);
            newShoot.Position = positionShoot.GlobalPosition;
            
        }
    }


    //--- Input move physics ---//

    private void InputAccelerateRigidBody()
    {
        if (PlayerDead == true) return;
        if (Input.IsActionPressed("w"))
        {
            //if (Position.Y > 11.5)
            //{
            //    CanDesacelerate = true;
            //    //ActiveAnimationIDLE();
            //    return; //clamp positon
            //}    

            Move_Y_RigidBody(1);
            ActiveAnimationUP();
            
        }
        if (Input.IsActionPressed("s"))
        {
            //if (Position.Y < -11.5)
            //{
            //    CanDesacelerate = true;
            //    //ActiveAnimationIDLE();
            //    return; //clamp positon
            //}

            Move_Y_RigidBody(-1);
            ActiveAnimationDOWN();
            
        }
        if (Input.IsActionJustReleased("s") || Input.IsActionJustReleased("w"))
        {
            CanDesacelerate = true;
            ActiveAnimationIDLE();
        }

        if (Input.IsActionPressed("d"))
        {
            //if (Position.X > 26)
            //{
            //    CanDesacelerate = true;
            //    //ActiveAnimationIDLE();
            //    return; //clamp positon
            //}

            Move_X_RigidBody(1);
        }
        if (Input.IsActionPressed("a"))
        {
            //if (Position.X < -25)
            //{
            //    CanDesacelerate = true;
            //    //ActiveAnimationIDLE();
            //    return; //clamp positon
            //}

            Move_X_RigidBody(-1);
        }
        if (Input.IsActionJustReleased("a") || Input.IsActionJustReleased("d"))
        {
            CanDesacelerate = true;
        }
    }


    //--- Move physics ---//

    private void Move_X_RigidBody(int direction)
    {
        velocity.Y = direction;
        Vector3 newvelocity = Vector3.Zero;
        newvelocity.Z = LinearVelocity.Z;
        newvelocity.Y = LinearVelocity.Y;
        newvelocity.X = VELOCITY_MOVEMENT * direction;
        LinearVelocity = newvelocity.Normalized() * VELOCITY_MOVEMENT;
        CanDesacelerate = false;
    }

    private void Move_Y_RigidBody(int direction)
    {
        velocity.Y = direction;
        Vector3 newvelocity = Vector3.Zero;
        newvelocity.Z = LinearVelocity.Z;
        newvelocity.X = LinearVelocity.X;
        newvelocity.Y = VELOCITY_MOVEMENT * direction;
        LinearVelocity = newvelocity.Normalized() * VELOCITY_MOVEMENT;
        CanDesacelerate = false;
    }

    private void DesacelerateRigidBody(PhysicsDirectBodyState3D state)
    {
        if (PlayerDead == true) return;
        if (CanDesacelerate)
        {
            float speed = state.LinearVelocity.Length();
            speed -= VELOCITY_DESACELERATION * (float)GetProcessDeltaTime();
            if (speed < 0.0f) speed = 0.0f;
            state.LinearVelocity = state.LinearVelocity.Normalized() * speed;
        }
    }


    //--- Animations ---//

    private void ActiveAnimationIDLE()
    {
        animations.Set("parameters/conditions/moverabajo", false);
        animations.Set("parameters/conditions/moverarriba", false);
        animations.Set("parameters/conditions/idle", true);
    }

    private void ActiveAnimationDOWN()
    {
        animations.Set("parameters/conditions/moverabajo", true);
        animations.Set("parameters/conditions/moverarriba", false);
        animations.Set("parameters/conditions/idle", false);
    }

    private void ActiveAnimationUP()
    {
        animations.Set("parameters/conditions/moverarriba", true);
        animations.Set("parameters/conditions/moverabajo", false);
        animations.Set("parameters/conditions/idle", false);
    }


    //--- Player Dead Respawn ---//

    private async Task Desactive_player(Area3D area)
    {
        PlayerDead = true;
        // fisicas
        Sleeping = true;
        CanDesacelerate = true;
        // creo nodo particulas
        GpuParticles3D newParticleExplosion = (GpuParticles3D)FIRE_EXPLOSION.Instantiate();
        newParticleExplosion.Position = GlobalPosition;
        GetTree().GetFirstNodeInGroup("main").AddChild(newParticleExplosion);
        newParticleExplosion.Emitting = true;
        // sonidos
        (GetTree().GetFirstNodeInGroup("explosionSound") as AudioStreamPlayer).Play();
        (GetTree().GetFirstNodeInGroup("hitSound") as AudioStreamPlayer).Play();
        // animaciones
        ActiveAnimationIDLE();
        GetNode<Node3D>("naveRig").Visible = false;
        // posicion
        Position = POSITION_RELOAD;
        // reinicio visibilidad
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");//detengo por 1 segundo
        GetNode<Node3D>("naveRig").Visible = true;
        //destruyo nodo particula creado anteriormente
        newParticleExplosion.QueueFree();

    }
    private void RespawnPosition(double delta)
    {
        if (PlayerDead)
        {
            Vector3 newposition = Position;
            newposition.X += VELOCITY_RELOAD * (float)delta;
            Position = newposition;
            if(Position.X > POSITION_FINAL_RELOAD_X)
            {
                Rotation = new Vector3(0, 0, 0);
                ActivatePlayer();
            }
        }
    }

    private void ActivatePlayer()
    {
        PlayerDead = false;
        Sleeping = false;
    }

   
    

}
