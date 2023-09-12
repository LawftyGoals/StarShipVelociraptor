using Godot;
using System;
using System.Diagnostics.Tracing;
using System.Dynamic;
using ResourceHelpers;
using System.ComponentModel;

public partial class StellarObject : Area2D
{
    [Export]
    protected Godot.Vector2 StellarParentPosition { get; set; } = new Godot.Vector2(0, 0);
    protected StellarObject StellarParent;
    protected float RotationMultiplyer { get; set; } = 1000;
    public string StellarObjectName { get; set; }
    public Godot.Vector2 StellarObjectPosition { get; set; } = new Godot.Vector2(1000, 0);
    protected int StellarObjectSize { get; set; } = 1000;
    public Boolean StellarObjectStatic { get; set; } = true;
    public int StellarObjectYear { get; set; } = 365;
    public int StellarObjectDay { get; set; } = 24;
    public int RotationDirection { get; set; } = 1;

    public StellarObject StellarObjectParent { get; set; } = null;
    private float _distanceToParent = 1000f;
    public float DistanceToParent
    {
        get => _distanceToParent;
        set { _distanceToParent = value + StellarParent.StellarObjectSize; }
    }

    protected Sprite2D _objectsSprite;
    protected CollisionShape2D _objectsCollision;

    protected string ImagePath { get; set; } = "res://images/planets/resizedfirstplanet.png";

    public StellarObject() { }

    public StellarObject(
        string objectName,
        Godot.Vector2 position,
        int size,
        int year,
        int day,
        int direction,
        Boolean stellarStatic,
        StellarObject parent = null,
        float parentDistance = 0
    )
    {
        StellarObjectName = objectName;
        StellarObjectPosition = position;
        StellarObjectSize = size;
        StellarObjectStatic = stellarStatic;
        StellarObjectYear = year;
        StellarObjectDay = day;
        RotationDirection = direction;

        StellarObjectParent = parent;
        DistanceToParent = parentDistance;
    }

    public override void _Process(double delta)
    {
        rotatePlanet((float)delta);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _objectsSprite = GetNode<Sprite2D>("StellarObjectSprite2D");
        _objectsCollision = GetNode<CollisionShape2D>("StellarObjectCollisionShape2D");

        initiateSprite();
        initiateCollision();
        setPosition();
    }

    protected void initiateSprite() =>
        _objectsSprite.Texture = new ImageLoader().LoadImageToTexture(ImagePath, StellarObjectSize);

    protected void initiateCollision() =>
        _objectsCollision.Shape = new CircleShape2D() { Radius = objectRadius() };

    public void setPosition() => Position = StellarObjectPosition;

    public int objectRadius() => StellarObjectSize / 2;

    protected void rotatePlanet(float delta) =>
        _objectsSprite.GlobalRotation += RotationDirection * rotationSpeed() * delta;

    protected float rotationSpeed() => StellarObjectDay / RotationMultiplyer;
}
