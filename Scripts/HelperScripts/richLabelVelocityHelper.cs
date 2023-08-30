using Godot;
using System;

public partial class richLabelVelocityHelper : RichTextLabel
{
    StarShipMovement parent;

    public override void _Ready()
    {
        parent = GetParent<StarShipMovement>();
    }

    public override void _Process(double delta)
    {
        this.Text =
            $"Velocity: {parent.Velocity} \nChildrotation: {parent._childCollisionPolygon.GlobalRotation} \nDirectionValue: {parent.directionValue()} \nCompare: {parent.compare} - {parent.compare.X + parent.compare.Y}";
    }
}
