using Godot;
using System;
using System.Numerics;

public partial class HelperScripts : Node
{
    public override void _Ready()
    {
        GD.Print();
    }

    Godot.Vector2 storedVelocity = new Godot.Vector2(0, 0);

    public void printVelocityChange(Godot.Vector2 velocity)
    {
        if (storedVelocity != velocity)
        {
            GD.Print(velocity);
            storedVelocity = velocity;
        }
    }
}
