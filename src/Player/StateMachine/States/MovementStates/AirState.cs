using System;
using Godot;
using Godot.Collections;

public class AirState : MovementState
{
    [Export] float acceleration_x = 180_000.0f;
    [Export] public float jump_impulse = 1_000;
    [Export] public float air_jump_impulse = 700;
    [Export] public int air_jump_max_count = 1;

    private int air_jump_count = 0;
    private bool can_air_jump = false;

    public override void enter()
    {
        can_air_jump = true;
        if (_owner.IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            jump(jump_impulse);
        }
    }

    public override void exit()
    {
        acceleration = acceleration_default;
        if (air_jump_count >= air_jump_max_count && _owner.IsOnFloor())
        {
            air_jump_count = 0;
            can_air_jump = false;
        }
    }

    public override void PhysicsProcess(float delta)
    {
        base.PhysicsProcess(delta);
        if (_owner.IsOnFloor())
        {
            String target_state_str =
                get_move_direction().x == 0 ?
                    typeof(IdleState).Name
                    : typeof(RunState).Name;
            _state_machine.transition_to(target_state_str);
        }
    }

    public override void UnhandledInput(InputEvent @event)
    {
        base.UnhandledInput(@event);
        // air jump
        if (@event.IsActionPressed("jump") && air_jump_count < air_jump_max_count && can_air_jump)
        {
            jump(air_jump_impulse);
            air_jump_count += 1;
        }
        else if (air_jump_count >= air_jump_max_count)
        {
            can_air_jump = false;
        }
    }

    public void jump(float impulse)
    {
        acceleration.x = acceleration_x;
        velocity.y = 0;
        max_speed.x = Mathf.Max(
            Mathf.Abs(velocity.y),
            max_speed_default.x
        );
        velocity += calculate_velocity(
            new Vector2(0, impulse),
            1,
            Vector2.Up
        );
    }
}
