using Godot;
using System;

public partial class player : RigidBody3D
{
    [Export]
    private float velocity_movement = 9f;
    [Export]
    private float velocity_desaceleration = 5f;

    [Export]
    private PackedScene ball;

    public Vector2 velocity = Vector2.Zero;
    private bool CanDesacelerate = false;
    private AnimationTree animations;
    private Vector3 positionShoot;

    
    public override void _Ready()
	{
        animations = GetNode<AnimationTree>("naveRig/AnimationTreeNave");
        positionShoot = GetNode<Node3D>("positionShoot").Position;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if(Input.IsActionJustPressed("space"))
        {
            shoot();
        }

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




    //--- input instantiate ball ---//
    private void shoot()
    {
        RigidBody3D newShoot = (RigidBody3D)ball.Instantiate();
        newShoot.Position = positionShoot;
        AddChild(newShoot);
    }


    //--- input move physics ---//
    private void InputAccelerateRigidBody()
    {
        if (Input.IsActionPressed("w"))
        {
            Move_Y_RigidBody(1);
            ActiveAnimationUP();
        }
        if (Input.IsActionPressed("s"))
        {
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
            Move_X_RigidBody(1);
        }
        if (Input.IsActionPressed("a"))
        {
            Move_X_RigidBody(-1);
        }
        if (Input.IsActionJustReleased("a") || Input.IsActionJustReleased("d"))
        {
            CanDesacelerate = true;
        }
    }


    //--- move physics ---//
    private void Move_X_RigidBody(int direction)
    {
        velocity.Y = direction;
        Vector3 newvelocity = Vector3.Zero;
        newvelocity.Z = LinearVelocity.Z;
        newvelocity.Y = LinearVelocity.Y;
        newvelocity.X = velocity_movement * direction;
        LinearVelocity = newvelocity.Normalized() * velocity_movement;
        CanDesacelerate = false;
    }

    private void Move_Y_RigidBody(int direction)
    {
        velocity.Y = direction;
        Vector3 newvelocity = Vector3.Zero;
        newvelocity.Z = LinearVelocity.Z;
        newvelocity.X = LinearVelocity.X;
        newvelocity.Y = velocity_movement * direction;
        LinearVelocity = newvelocity.Normalized() * velocity_movement;
        CanDesacelerate = false;
    }

    private void DesacelerateRigidBody(PhysicsDirectBodyState3D state)
    {
        if (CanDesacelerate)
        {
            float speed = state.LinearVelocity.Length();
            speed -= velocity_desaceleration * (float)GetProcessDeltaTime();
            if (speed < 0.0f) speed = 0.0f;
            state.LinearVelocity = state.LinearVelocity.Normalized() * speed;
        }
    }


    //--- animations ---//

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


}
