using Godot;
using System;
using System.Dynamic;

public partial class CameraScript : Camera2D
{
    private Godot.Vector2 setZoomLevel = new Godot.Vector2(1, 1);

    private Godot.Vector2 maxZoomLevel = new Godot.Vector2(2, 2);
    private Godot.Vector2 minZoomLevel = new Godot.Vector2(0.5f, 0.5f);

    private Godot.Vector2 zoomSkip = new Godot.Vector2(0.1f, 0.1f);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            switch (mouseEvent.ButtonIndex)
            {
                case MouseButton.WheelDown:
                    zoomOut();
                    break;
                case MouseButton.WheelUp:
                    zoomIn();
                    break;
            }
        }
    }

    private void zoomIn()
    {
        if (setZoomLevel <= maxZoomLevel)
            setZoomLevel += zoomSkip;
    }

    private void zoomOut()
    {
        if (setZoomLevel >= minZoomLevel)
        {
            setZoomLevel -= zoomSkip;
        }
    }

    public void smoothZoom(double delta)
    {
        float editedDelta = (float)Math.Round(1 * delta, 2);

        if (setZoomLevel > Zoom)
        {
            Zoom += new Godot.Vector2(editedDelta, editedDelta);
        }
        if (setZoomLevel < Zoom)
        {
            Zoom -= new Godot.Vector2(editedDelta, editedDelta);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        smoothZoom(delta);
    }
}
