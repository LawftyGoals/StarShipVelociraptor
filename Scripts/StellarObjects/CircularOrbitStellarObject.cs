using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Transactions;
using static HelperScripts;

public partial class CircularOrbitStellarObject : StaticStellarObject
{
    private HelperScripts hp = new HelperScripts();
    private Godot.Vector2 origin = new Godot.Vector2(0, 0);

    public float OrbitSpeed { get; set; } = 1f; // 10 at 3k is too small to make an actual differnce. 11.9 seems to be a minimum? >.<
    public float OrbitDirection { get; set; } = 1f;

    public CircularOrbitStellarObject() { }

    public CircularOrbitStellarObject(
        string objectName,
        int size,
        int year,
        int day,
        int direction,
        float orbitSpeed = 12f,
        float orbitDirection = 1f,
        Boolean landable = false,
        StaticStellarObject parent = null,
        float parentDistance = 0
    )
    {
        StellarObjectName = objectName;
        StellarObjectSize = size;
        StellarObjectYear = year;
        StellarObjectDay = day;
        RotationDirection = direction;
        OrbitSpeed = orbitSpeed;
        OrbitDirection = orbitDirection;
        Landable = landable;
        StellarObjectParent = parent;
        DistanceToParent = parentDistance;
    }

    public override void _Ready()
    {
        BodyShapeEntered += printOnCollide;
        generateSpriteAndCollision();
        setPosition();
    }

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

        //hp.printDictionaryValues(
        //    new Dictionary<string, string>
        //    {
        //        { "Position", Position.ToString() },
        //        { "tempPosition", (Position - new Godot.Vector2(proposedX, proposedY)).ToString() },
        //        { "delta", delta.ToString() }
        //    }
        //);

        Position = new Godot.Vector2(proposedX, proposedY);
    }

    private float distanceBetweenPoints(Godot.Vector2 target, Godot.Vector2 parent) =>
        (float)Math.Sqrt(Math.Pow(target.X - parent.X, 2) + Math.Pow(target.Y - parent.Y, 2));

    private float findM(float x, float y) => y / x;

    private float distanceSolveForX(float distance, float m) =>
        (float)Math.Sqrt(Math.Pow(distance, 2) / (Math.Pow(m, 2) + 1));

    private float distanceSolveForY(float distance, float x) =>
        (float)Math.Sqrt(Math.Pow(distance, 2) - Math.Pow(x, 2));

    private float solveForCrossingTheLine(float x, float y, float m) => y - m * x;
}
