using System;
using Godot;
/// <summary>
/// Rig to move a child camera based on the player's input, to give them more forward visibility
/// </summary>
public class CameraRig : Position2D
{
    private Camera2D _camera;

    [Export] Vector2 offset = new Vector2(300, 300);
    [Export] Vector2 mouse_range = new Vector2(100, 500);

    private bool _is_active = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _camera = GetNode<Camera2D>("ShakingCamera");
    }

    public override void _PhysicsProcess(float delta)
    {
        update_position();
    }


    /// <summary>
    /// Updates the camera rig's position based on the player's state and controller position"
    /// </summary>
    /// <param name="velocity">Camera's velocity</param>
    public void update_position(Vector2? velocity = null)
    {
        if (velocity == null) velocity = Vector2.Zero;
        if (_is_active)
        {
            Vector2 mouse_position = GetGlobalMousePosition();
            float distance_ratio = Mathf.Clamp(
                mouse_position.Length(), mouse_range.x, mouse_range.y
                ) / mouse_range.y;
            _camera.Position = distance_ratio * mouse_position.Normalized() * offset;
        }
    }
}
