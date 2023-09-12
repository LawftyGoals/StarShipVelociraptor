using Godot;
using System;
using System.ComponentModel;
using System.Transactions;

public partial class CircularOrbitStellarObject : StellarObject
{
    private Godot.Vector2 origin = new Godot.Vector2(0, 0);

    public float OrbitSpeed { get; set; } = 10f;
    public float OrbitDirection { get; set; } = 1f;

    public CircularOrbitStellarObject() { }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //_objectsSprite = GetNode<Sprite2D>("StellarObjectSprite2D");
        //_objectsCollision = GetNode<CollisionShape2D>("StellarObjectCollisionShape2D");
        generateSpriteAndCollision();
        initiateSprite();
        initiateCollision();
        setPosition();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        rotatePlanet((float)delta);
        circularOrbitMovement((float)delta);
    }

    private void circularOrbitMovement(float delta)
    {
        Godot.Vector2 tempPosition = Position;
        Godot.Vector2 positiveOrNegative = new Godot.Vector2(0, 0);

        if (tempPosition.X <= 0 && tempPosition.Y >= 0)
            positiveOrNegative = new Godot.Vector2(-1, -1);

        if (tempPosition.X <= 0 && tempPosition.Y <= 0)
            positiveOrNegative = new Godot.Vector2(1, -1);

        if (tempPosition.X >= 0 && tempPosition.Y >= 0)
            positiveOrNegative = new Godot.Vector2(-1, 1);

        if (tempPosition.X >= 0 && tempPosition.Y <= 0)
            positiveOrNegative = new Godot.Vector2(1, 1);

        float currentM = findM(tempPosition.X, tempPosition.Y);

        float negativeM = -(1 / currentM);

        float step = OrbitSpeed;

        float perpendicularX = distanceSolveForX(step, negativeM);
        float perpendicularY = distanceSolveForY(step, perpendicularX);

        tempPosition.X += positiveOrNegative.X * Math.Abs(perpendicularX) * delta * OrbitDirection;
        tempPosition.Y += positiveOrNegative.Y * Math.Abs(perpendicularY) * delta * OrbitDirection;

        float afterStepXNoP = tempPosition.X < 0 ? -1 : 1;
        float afterStepYNoP = tempPosition.Y < 0 ? -1 : 1;

        float proposedM = findM(tempPosition.X, tempPosition.Y);

        float proposedX = afterStepXNoP * distanceSolveForX(DistanceToParent, proposedM);
        float proposedY = afterStepYNoP * distanceSolveForY(DistanceToParent, proposedX);

        Position = new Godot.Vector2(proposedX, proposedY);
    }

    private float distanceBetweenPoints(Godot.Vector2 target, Godot.Vector2 parent)
    {
        return (float)
            Math.Sqrt(Math.Pow(target.X - parent.X, 2) + Math.Pow(target.Y - parent.Y, 2));
    }

    private float findM(float x, float y)
    {
        return y / x;
    }

    private float distanceSolveForX(float distance, float m)
    {
        return (float)Math.Sqrt(Math.Pow(distance, 2) / (Math.Pow(m, 2) + 1));
    }

    private float distanceSolveForY(float distance, float x)
    {
        return (float)Math.Sqrt(Math.Pow(distance, 2) - Math.Pow(x, 2));
    }

    private float solveForCrossingTheLine(float x, float y, float m)
    {
        return y - m * x;
    }
}
