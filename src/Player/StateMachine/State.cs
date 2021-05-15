using System;
using Godot;
using Godot.Collections;
// We cannot set icons to appear in the Editor with the related node script object with C# 

/// <summary>
/// State interface to use in hierarchical State machines
/// </summary>
public class State : Node
{
    protected StateMachine _state_machine;

    // Called when the node enters the scene tree for the first time.
    public void Ready()
    {
        _state_machine = _get_state_machine(this);
    }

    public void PhysicsProcess(float delta)
    {
    }
    public void UnhandledInput(InputEvent @event)
    {

    }

    public void enter(Dictionary msg = null)
    {

    }

    public void exit()
    {

    }


    public StateMachine _get_state_machine(Node node)
    {
        if (node != null && node.IsInGroup("state_machine"))
        {
            return _get_state_machine(node.GetParent());
        }
        else return (StateMachine)node;
    }
}
