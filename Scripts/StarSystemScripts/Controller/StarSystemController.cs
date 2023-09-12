using Godot;
using System;

public partial class StarSystemController : Node
{
    // Called when the node enters the scene tree for the first time.

    protected StellarObject CentralStar { get; set; }

    public override void _Ready()
    {
        //PackedScene Scene = GD.Load<PackedScene>("res://Scenes/Environments/StellarObjects.tscn");
        CentralStar = new StellarObject("star", new Godot.Vector2(0, 0), 1500, 0, 15, 1, true)
        {
            ImagePath = "res://images/planets/SunPRototype.png"
        };
        AddChild(CentralStar);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
