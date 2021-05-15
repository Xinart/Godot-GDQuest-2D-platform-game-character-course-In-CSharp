using System;
using Godot;
using Godot.Collections;
/// <summary>
/// Shakes the screen when is_shaking is set to true
/// To make it react to events happening in the game world, use the Events signal routing singleton
/// Uses different smoothing values depending on the active controller
/// </summary>
public class ShakingCamera : Camera2D
{
    private Timer _timer;

    #region Export fields
    [Export] public float amplitude = 4;
    [Export] float DAMP_EASING = 1;
    [Export]
    Dictionary<string, float> default_smoothing_speed
     = new Dictionary<string, float>()
     {
         ["mouse"] = 3,
         ["gamepad"] = 1
     };
    private float __duration;
    [Export]
    float duration
    {
        get => __duration;
        set
        {
            __duration = value;
            if (_timer != null) _timer.WaitTime = duration;
        }
    }
    private bool __is_shaking;
    [Export]
    bool is_shaking
    {
        get => __is_shaking;
        set
        {
            __is_shaking = value;
            if (__is_shaking) _change_state(State.SHAKING);
            else _change_state(State.IDLE);
        }
    }
    #endregion

    enum State { IDLE, SHAKING }
    private State _state = State.IDLE;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");

        _timer.Connect("timeout", this, "_on_ShakeTimer_timeout");
        this.duration = duration;
        reset_smoothing_speed();
        SetProcess(false);
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var damping = Mathf.Ease(_timer.TimeLeft / _timer.WaitTime,
            DAMP_EASING);
        Offset = new Vector2(
            (float)GD.RandRange(amplitude, -amplitude) * damping,
            (float)GD.RandRange(amplitude, -amplitude) * damping
        );
    }

    public override string _GetConfigurationWarning()
    {
        return _timer == null ?
            $"{Name} requires a Timer child named Timer"
            : "";
    }

    private void reset_smoothing_speed()
    {
        SmoothingSpeed = default_smoothing_speed["mouse"];
    }

    public void _on_ShakeTimer_timeout()
    {
        is_shaking = false;
    }

    private void _change_state(State new_state)
    {
        switch (new_state)
        {
            case State.IDLE:
                Offset = new Vector2();
                SetProcess(false);
                break;
            case State.SHAKING:
                SetProcess(true);
                _timer.Start();
                break;
        }
        _state = new_state;
    }
}
