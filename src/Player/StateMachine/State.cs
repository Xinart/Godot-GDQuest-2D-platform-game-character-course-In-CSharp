using System;
using Godot;
using Godot.Collections;
// We cannot set icons to appear in the Editor with the related node script object with C# 

/// <summary>
/// Base class to use for states
/// </summary>
public abstract class State : Node
{
    /// <summary>
    /// State machine handling the state
    /// </summary>
    protected StateMachine _state_machine;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _state_machine = _get_state_machine(this);
    }

    /// <summary>
    /// Called by the state machine in the _PhysicsProcess engine method
    /// if this state is currently handled by the machine state
    /// </summary>
    public abstract void PhysicsProcess(float delta);

    /// <summary>
    /// Called by the state machine in the _UnhandledInput engine method
    /// if this state is currently handled by the machine state
    /// </summary>
    public abstract void UnhandledInput(InputEvent @event);

    private StateMachine _get_state_machine(Node node)
    {
        if (node != null && !node.IsInGroup("state_machine"))
        {
            return _get_state_machine(node.GetParent());
        }
        else return (StateMachine)node;
    }

    /// <summary>
    /// Define actions to do when we enter in this state
    /// </summary>
    public abstract void enter();

    /// <summary>
    /// Define actions to do when we exit this state
    /// </summary>
    public abstract void exit();
}
