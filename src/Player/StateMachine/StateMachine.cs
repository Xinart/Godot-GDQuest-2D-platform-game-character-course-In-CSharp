using System;
using Godot;
using Godot.Collections;

/// <summary>
/// Initializes states and delegate engine callbacks to the active states
/// </summary>
public class StateMachine : Node
{
    [Export] NodePath initial_state = new NodePath();

    public String state_name;
    private State __state;
    public State state
    {
        get => __state;
        set
        {
            __state = value;
            state_name = __state.Name;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        state = GetNode<State>(initial_state);
        await ToSignal(Owner, "ready");
        state.enter();
    }

    public override void _PhysicsProcess(float delta)
    {
        state.PhysicsProcess(delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        state.UnhandledInput(@event);
    }

    public void _init()
    {
        AddToGroup("state_machine");
    }

    public void transition_to(String target_state_path, Dictionary msg = null)
    {
        if (HasNode(target_state_path))
        {
            State target_state = GetNode<State>(target_state_path);

            state.exit();
            state = target_state;
            state.enter(msg);
        }
    }


}
