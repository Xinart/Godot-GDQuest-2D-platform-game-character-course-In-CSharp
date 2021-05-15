using System;
using Godot;

public class HookTarget : Area2D
{
    [Export] bool is_one_shoot = false;

    [Signal] delegate void hooked_onto_from(Vector2 hook_position);

    readonly Color COLOR_ACTIVE = new Color(0.9375f, 0.730906f, 0.025635f);
    readonly Color COLOR_INACTIVE = new Color(0.515625f, 0.484941f, 0.4552f);

    private Timer _timer;

    private Color __color;
    public Color color
    {
        get => __color; set
        {
            __color = value;
            Update();
        }
    }
    private bool __is_active = true;
    public bool is_active
    {
        get => __is_active;
        set
        {
            __is_active = value;
            __color = __is_active ? COLOR_ACTIVE : COLOR_INACTIVE;
            if (!__is_active && !is_one_shoot)
            {
                _timer.Start();
            }
        }
    }

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Connect("timeout", this, "_on_Timer_timeout");
        color = COLOR_ACTIVE;
    }

    public void _on_Timer_timeout()
    {
        __is_active = true;
    }
    public override void _Draw()
    {
        DrawCircle(Vector2.Zero, 12, __color);
    }

    public void hooked_from(Vector2 hook_position)
    {
        __is_active = false;
        EmitSignal("hooked_onto_from", hook_position);
    }
}
