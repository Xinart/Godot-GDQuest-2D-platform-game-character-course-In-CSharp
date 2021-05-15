using System;
using Godot;

public class Events : Node
{
    [Signal] delegate void player_moved(Node player);
}
