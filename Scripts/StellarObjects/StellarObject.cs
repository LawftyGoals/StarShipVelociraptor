using Godot;
using System;
using System.Diagnostics.Tracing;

public partial class StellarObject : Node
{
    [Export]
    public float RotationMultiplyer { get; set; } = 1000;
    public string StellarObjectName { get; set; }
    public int StellarObjectSize { get; set; } = 50;

    public Godot.Vector2 StellarObjectPosition { get; set; }

    public Boolean StellarObjectStatic { get; set; } = true;
    public int StellarObjectYear { get; set; } = 365;
    public int StellarObjectHour { get; set; } = 24;
    public int RotationDirection { get; set; } = 1;
    public StellarObject StellarObjectParent { get; set; } = null;

    private Sprite2D _objectsSprite;
    private CollisionShape2D _objectsCollision;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print(StellarObjectHour / RotationMultiplyer);
        _objectsSprite = GetNode<Sprite2D>("StellarObjectSprite2D");
        _objectsCollision = GetNode<CollisionShape2D>("StellarObjectCollisionShape2D");
        GD.Print(_objectsSprite.Texture.GetSize());
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        rotatePlanet((float)delta);
    }

    private void rotatePlanet(float delta)
    {
        _objectsSprite.GlobalRotation += RotationDirection * rotationSpeed() * delta;
    }

    private float rotationSpeed()
    {
        return StellarObjectHour / RotationMultiplyer;
    }
}
