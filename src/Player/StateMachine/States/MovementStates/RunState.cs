using System;
using Godot;
using Godot.Collections;
/// <summary>
/// Horizontal movement on the ground.
// Delegates movement to its parent Move state and extends it
// with state transitions
/// </summary>
public class RunState : MovementState
{
    [Export] float sprint_max_speed_x = 1000;

    public override void enter()
    {
        max_speed = max_speed_default;
    }

    public override void exit()
    {
        max_speed = max_speed_default;
    }

    public override void PhysicsProcess(float delta)
    {
        if (_owner.IsOnFloor() && get_move_direction().x == 0.0)
        {
            _state_machine.transition_to(typeof(IdleState).Name);
        }
        else if (!_owner.IsOnFloor())
        {
            _state_machine.transition_to(typeof(AirState).Name);
        }
        base.PhysicsProcess(delta);
    }

    public override void UnhandledInput(InputEvent @event)
    {
        base.UnhandledInput(@event);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Input.IsActionPressed("sprint"))
        {
            max_speed.x = sprint_max_speed_x;
        }
        else max_speed = max_speed_default;
    }
}
