using Godot;
using System;
using System.Numerics;

public partial class HelperScripts : Node
{
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
