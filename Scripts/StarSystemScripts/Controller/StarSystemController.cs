using Godot;
using System;

public partial class StarSystemController : Node
{
    // Called when the node enters the scene tree for the first time.

    protected StellarObject CentralStar { get; set; }

    public override void _Ready()
    {
        PackedScene Scene = GD.Load<PackedScene>("res://Scenes/Environments/StellarObjects.tscn");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
