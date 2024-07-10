using Godot;
using System;

public partial class UniversalEffectiveValues : Node
{
    private double _universalDelta = 0;
    public double UniversalDelta
    {
        get => _universalDelta;
    }

    private Boolean paused = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        _universalDelta = paused ? 0 : delta;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
            if (eventKey.Pressed && eventKey.Keycode == Key.Space && !eventKey.Echo)
            {
                GetTree().Paused = !GetTree().Paused;
            }
    }
}
