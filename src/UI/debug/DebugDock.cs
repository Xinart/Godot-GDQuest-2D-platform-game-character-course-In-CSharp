using System;
using Godot;

/// <summary>
/// Contains UI widgets that display debug info about the game world
/// </summary>
public class DebugDock : MarginContainer
{
    public override void _GuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("toggle_debug_menu"))
        {
            this.Visible = !this.Visible;
            AcceptEvent();
        }
    }
}
