using System;
using Godot;
using Godot.Collections;

public class IdleState : MovementState
{
    public override void enter()
    {
        max_speed = max_speed_default;
        velocity.y = Vector2.Zero.y;
    }

    public override void exit()
    {

    }

    public override void PhysicsProcess(float delta)
    {
        base.PhysicsProcess(delta);
        if (_owner.IsOnFloor() && get_move_direction().x != 0.0)
        {
            _state_machine.transition_to(typeof(RunState).Name);
        }
        else if (!_owner.IsOnFloor())
        {
            _state_machine.transition_to(typeof(AirState).Name);
        }
        // Allow friction on x axis to finish to 0 on the physic process handled by the move state
    }

    public override void UnhandledInput(InputEvent @event)
    {
        base.UnhandledInput(@event);
    }
}
