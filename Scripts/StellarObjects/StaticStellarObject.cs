using Godot;
using System;
using System.Diagnostics.Tracing;
using System.Dynamic;
using ResourceHelpers;
using System.ComponentModel;

public partial class StaticStellarObject : Area2D
{
    //CURRENT
    [Export]
    protected float RotationMultiplyer { get; set; } = 1000;
    public string StellarObjectName { get; set; }
    public Godot.Vector2 StellarObjectPosition { get; set; } = new Godot.Vector2(1000, 0);
    protected int StellarObjectSize { get; set; } = 1000;
    public int StellarObjectYear { get; set; } = 365;
    public int StellarObjectDay { get; set; } = 24;
    public int RotationDirection { get; set; } = 1;

    //Landable
    public Boolean Landable { get; set; } = false;
    protected CollisionShape2D _landingZone = null;
    public CollisionShape2D LandingZone
    {
        get => _landingZone;
        protected set => _landingZone = value;
    }

    //PARENT

    protected Godot.Vector2 StellarParentPosition { get; set; } = new Godot.Vector2(0, 0);
    public StaticStellarObject StellarObjectParent { get; set; } = null;
    private float _distanceToParent = 1000f;
    public float DistanceToParent
    {
        get => _distanceToParent;
        set
        {
            _distanceToParent =
                StellarObjectParent == null ? value : value + StellarObjectParent.StellarObjectSize;
        }
    }

    protected Sprite2D _objectsSprite;
    protected CollisionShape2D _objectsCollision;

    public string ImagePath { get; set; } = "res://images/planets/resizedfirstplanet.png";

    public StaticStellarObject() { }

    public StaticStellarObject(
        string objectName,
        Godot.Vector2 position,
        int size,
        int year,
        int day,
        int direction,
        Boolean landable = false,
        StaticStellarObject parent = null,
        float parentDistance = 0
    )
    {
        StellarObjectName = objectName;
        StellarObjectPosition = position;
        StellarObjectSize = size;
        StellarObjectYear = year;
        StellarObjectDay = day;
        RotationDirection = direction;
        Landable = landable;
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
        BodyEntered += printOnCollide;
        generateSpriteAndCollision();
        setPosition();
    }

    private void printOnCollide(Node2D body)
    {
        GD.Print("Collided");
    }

    protected void generateSpriteAndCollision()
    {
        _objectsSprite = new Sprite2D
        {
            Name = "StellarObjectSprite2D",
            Texture = new ImageLoader().LoadImageToTexture(ImagePath, StellarObjectSize)
        };
        AddChild(_objectsSprite);

        _objectsCollision = new CollisionShape2D
        {
            Name = "StellarObjectCollisionShape2D",
            Shape = new CircleShape2D() { Radius = objectRadius() }
        };
        AddChild(_objectsCollision);

        if (Landable)
        {
            LandingZone = new CollisionShape2D
            {
                Name = "StellarObjectLandingCollison2D",
                Shape = new CircleShape2D()
                {
                    Radius = objectRadius() + (int)(objectRadius() * 0.1)
                }
            };

            AddChild(LandingZone);
        }
    }

    public void setPosition() => Position = StellarObjectPosition;

    public int objectRadius() => StellarObjectSize / 2;

    protected void rotatePlanet(float delta) =>
        _objectsSprite.GlobalRotation += RotationDirection * rotationSpeed() * delta;

    protected float rotationSpeed() => StellarObjectDay / RotationMultiplyer;
}
