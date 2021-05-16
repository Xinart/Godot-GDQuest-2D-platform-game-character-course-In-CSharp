using System;
using Godot;

public class Player : KinematicBody2D
{
    private StateMachine _state_machine;
    private CollisionShape2D _collider;
    public readonly Vector2 FLOOR_NORMAL = Vector2.Up;
    private bool __is_active = true;
    public bool is_active
    {
        get => __is_active;
        set
        {
            __is_active = value;
            if (_collider != null) _collider.Disabled = !value;
        }
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _state_machine = GetNode<StateMachine>("StateMachine");
        _collider = GetNode<CollisionShape2D>("CollisionShape2D");
    }
}
