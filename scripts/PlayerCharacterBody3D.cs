using Godot;
using System;


public partial class PlayerCharacterBody3D : Godot.CharacterBody3D
{
    //[Signal]
    // public delegate void CoinCollected(int coins);
    [Export]
    int movement_speed = 250;
    [Export]
    float jump_strength = 7;
    [Export]
    float rotationSpeed = 7;
    [Export]
    float deltaRotation = 0;

    private Quaternion targetRotation = Quaternion.Identity;
    

    Vector3 movement_velocity;
    float rotation_direction;
    float gravity = 0;
    bool previously_floored = false;
    [Export]
    bool jump_single = true;
    [Export]
    bool jump_double = true;

    public override void _PhysicsProcess(double delta)
    {
        HandleControls(delta);
        HandleGravity(delta);
        // HandleEffects();

        Vector3 appliedVelocity = movement_velocity.Lerp(Velocity, (float)delta * 10);

        appliedVelocity.Y = -gravity;
        Velocity = appliedVelocity;
        
        RotatePlayer(delta);

        Scale = Scale.Lerp(new Vector3(1, 1, 1), (float)delta * 10);

        previously_floored = IsOnFloor();

        MoveAndSlide();
    }

    void RotatePlayer(double delta) {

            if (Velocity != Vector3.Zero) {
                var current_rot = GlobalTransform.Basis.GetRotationQuaternion().Normalized();
                rotation_direction = new Vector2(Velocity.Z, Velocity.X).Angle();
                var quat = new Quaternion(GlobalTransform.Basis.Y.Normalized(), rotation_direction);
                var smoothrot = current_rot.Slerp(quat, deltaRotation * rotationSpeed);
                GlobalTransform = new Transform3D( new Basis(smoothrot), GlobalTransform.Origin);
            }
    }

    void HandleControls(double delta)
    {
        Vector3 input = Vector3.Zero;

        input.X = Input.GetActionStrength("move_left") - Input.GetActionStrength("move_right");
        input.Z = Input.GetActionStrength("move_forward") - Input.GetActionStrength("move_back");

        movement_velocity = input.Normalized() * movement_speed * (float)delta;

        if (Input.IsActionJustPressed("jump"))
        {
            if (jump_double)
            {
                gravity = -jump_strength;
                jump_double = false;
                Scale = new Vector3(0.5f, 1.5f, 0.5f);
            }

            if (jump_single)
            {
                Jump();
            }
        }
    }

    void HandleGravity(double delta)
    {
        gravity += 25 * (float)delta;

        if (gravity > 0 && IsOnFloor())
        {
            jump_single = true;
            gravity = 0;
        }
    }

    void Jump()
    {
        gravity = -jump_strength;
        Scale = new Vector3(0.5f, 1.5f, 0.5f);
        jump_single = false;
        jump_double = true;
    }
}
