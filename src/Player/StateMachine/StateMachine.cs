using System;
using Godot;
using Godot.Collections;
// We cannot set icons to appear in the Editor with the related node script object with C# 

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

    public StateMachine()
    {
        // Add the state machine to his dedicated group to recognize it from the states
        AddToGroup("state_machine");
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

    public void transition_to(String target_state_name)
    {
        State target_state = get_state(GetChildren(), target_state_name);
        // GetNode<State>("MoveStates/" + target_state_name);
        // get_state(this, target_state_name)

        state.exit();
        state = target_state;
        state.enter();

    }

    public State get_state(Godot.Collections.Array state_list, String target_state_name)
    {
        foreach (State state in state_list)
        {
            // Check name of the script attached to the Node
            if (state.GetType().Name == target_state_name)
            {
                return state;
            }
            else if (state.GetChildren().Count != 0)
            {
                return get_state(state.GetChildren(), target_state_name);
            }
        }
        return null;
    }
}
