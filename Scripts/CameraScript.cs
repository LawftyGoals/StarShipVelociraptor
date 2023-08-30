using Godot;
using System;

public partial class CameraScript : Node2D
{
    CharacterBody2D playerShip;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        playerShip = GetNode<CharacterBody2D>("PlayerCharacterBody2D");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
