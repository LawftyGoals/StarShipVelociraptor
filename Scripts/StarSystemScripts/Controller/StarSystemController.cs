using Godot;
using System;

public partial class StarSystemController : Node
{
    // Called when the node enters the scene tree for the first time.

    protected StaticStellarObject CentralStar { get; set; }

    protected CircularOrbitStellarObject OrbitingPlanet { get; set; }

    public override void _Ready()
    {
        //PackedScene Scene = GD.Load<PackedScene>("res://Scenes/Environments/StellarObjects.tscn");
        CentralStar = new StaticStellarObject("star", new Godot.Vector2(0, 0), 1500, 0, 15, 1)
        {
            ImagePath = "res://images/planets/SunPRototype.png"
        };

        OrbitingPlanet = new CircularOrbitStellarObject(
            "stinky",
            326,
            52,
            52,
            1,
            12f,
            1f,
            true,
            CentralStar
        )
        {
            DistanceToParent = 2000f,
            Landable = true,
        };

        OrbitingPlanet.StellarObjectPosition = new Godot.Vector2(
            OrbitingPlanet.DistanceToParent,
            0
        );
        //OrbitingPlanet.DistanceToParent = 3000f;
        AddChild(CentralStar);
        CentralStar.AddChild(OrbitingPlanet);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
