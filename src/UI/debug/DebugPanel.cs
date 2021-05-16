using System;
using System.Linq;
using Godot;

/// <summary>
/// Displays the values of properties of a given node
/// You can directly change the `properties` property to display multiple values from the `reference` node
/// E.g. properties = PoolStringArray(['speed', 'position', 'modulate'])
/// </summary>
[Tool]
public class DebugPanel : PanelContainer
{
    #region Export fields
    [Export] NodePath reference_path;
    String[] __properties;
    [Export]
    String[] properties
    {
        get => __properties; set
        {
            __properties = value;
            if (reference != null) _setup();
        }
    }
    #endregion

    private Node __reference;
    public Node reference
    {
        get => __reference; set
        {
            __reference = value;
            if (__reference != null) _setup();
            else _title.Text = GetClass();
        }
    }
    // Fields to init on the _Ready method
    private VBoxContainer _container;
    private Label _title;


    #region overriden methods
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _container = GetNode<VBoxContainer>("VBoxContainer/MarginContainer/VBoxContainer");
        _title = GetNode<Label>("VBoxContainer/ReferenceName");
        reference = GetNode<Node>(reference_path);
        if (reference != null) _setup();
    }

    public override void _Process(float delta)
    {
        _update();
    }

    public override string _GetConfigurationWarning()
    {
        return reference_path.IsEmpty() ?
            "Reference Path should not be empty." : "";
    }
    #endregion p

    private void _setup()
    {
        _clear();
        _title.Text = reference.Name;
        foreach (String property in properties) _track(property);
    }

    /// Add a label in the panel, associated to the property param
    private void _track(String property)
    {
        Label label = new Label();
        label.Autowrap = true;
        label.Name = property.Capitalize();
        _container.AddChild(label);
        if (!properties.Contains(property)) properties.Append(property);
    }


    private void _update()
    {
        if (Engine.EditorHint) return;
        Array search_array = properties;
        foreach (String property in properties)
        {
            Label label = _container.GetChild<Label>(Array.IndexOf(search_array, property));
            var value = reference.Get(property);

            string text = "";
            if (value is Vector2) text = $"(x: {((Vector2)value).x}, y: {((Vector2)value).y}";
            else text = GD.Str(value);
            label.Text = $"{property.Capitalize()}: {text}";
        }
    }

    private void _clear()
    {
        foreach (Label property_label in _container.GetChildren())
        {
            property_label.QueueFree();
        }
    }



}
