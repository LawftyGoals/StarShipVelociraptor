using Godot;
using System;
using System.ComponentModel;

public partial class MovableStellarObject : StellarObject
{
    protected Godot.Vector2 ParentPosition { get; set; } = new Godot.Vector2(0, 0);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _objectsSprite = GetNode<Sprite2D>("StellarObjectSprite2D");
        _objectsCollision = GetNode<CollisionShape2D>("StellarObjectCollisionShape2D");

        initiateSprite();
        initiateCollision();
        setPosition();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        rotatePlanet((float)delta);
    }

    private void movePlanet()
    {
        Godot.Vector2 tempPosition;
    }
}
