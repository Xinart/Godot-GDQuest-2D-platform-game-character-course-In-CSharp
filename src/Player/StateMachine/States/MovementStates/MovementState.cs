using System;
using Godot;
using Godot.Collections;

/// <summary>
/// Category state that define the basic behaviour of movement states of the Player
/// This state must not be called DIRECTLY by the state machine
/// This state must be only inherited and used by the childrenmovement states
/// </summary>
public class MovementState : State
{
    // /!\ making static fields allow us to retrieve exactly the SAME DATA fields
    // across the childrens instance object

    protected static Vector2 max_speed_default = new Vector2(500, 1500);
    protected static Vector2 acceleration_default = new Vector2(100_000, 3000);

    protected static Vector2 acceleration;
    protected static Vector2 max_speed;
    protected static Vector2 velocity = Vector2.Zero;

    protected static Player _owner;

    public MovementState()
    {
        Name = GetType().Name;
    }

    #region overidden methods
    public override async void _Ready()
    {
        base._Ready();
        await ToSignal(Owner, "ready");
        _owner = Owner as Player;
        acceleration = acceleration_default;
        max_speed = max_speed_default;
    }

    public override void PhysicsProcess(float delta)
    {
        velocity = calculate_velocity(acceleration, delta, get_move_direction());
        velocity = _owner.MoveAndSlide(velocity, _owner.FLOOR_NORMAL);
        Events events = (Events)GetNode("/root/Events");
        events.EmitSignal("player_moved", _owner);
    }

    public override void UnhandledInput(InputEvent @event)
    {
        if (_owner.IsOnFloor() && @event.IsActionPressed("jump"))
        {
            _state_machine.transition_to(typeof(AirState).Name);
        }
    }
    #endregion

    public Vector2 calculate_velocity(
        Vector2 acceleration,
        float delta,
        Vector2 move_direction
    )
    {
        Vector2 new_velocity = velocity;
        if (move_direction.x != 0) // when Right/left key are pressed, let's accelerate the X axis of the velocity
        {
            new_velocity.x = Mathf.Lerp(new_velocity.x, move_direction.x * acceleration.x * delta, 0.05f);
        }
        else if (_state_machine.state_name == typeof(AirState).Name) // No move input in the air state, so we decelerate on x axis
        {
            float deceleration_air_state = 0.03f;
            new_velocity.x = Mathf.Lerp(new_velocity.x, 0, deceleration_air_state);
        }
        else // No move input, deceleration on x axis
        {
            float deceleration_idle_state = 0.35f;
            new_velocity.x = Mathf.Lerp(new_velocity.x, 0, deceleration_idle_state);
        }
        new_velocity.y += move_direction.y * acceleration.y * delta;

        new_velocity.x = Mathf.Clamp(new_velocity.x, -max_speed.x, max_speed.x);
        new_velocity.y = Mathf.Clamp(new_velocity.y, -max_speed.y, max_speed.y);

        return new_velocity;
    }

    public Vector2 get_move_direction()
    {
        return new Vector2(
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left")
            , 1
        );
    }

    public override void enter()
    {
    }

    public override void exit()
    {
    }
}
