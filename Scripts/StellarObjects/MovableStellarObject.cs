using Godot;
using System;
using System.ComponentModel;
using System.Transactions;

public partial class MovableStellarObject : StellarObject
{
    private Godot.Vector2 origin = new Godot.Vector2(0, 0);
    protected Godot.Vector2 ParentPosition { get; set; } = new Godot.Vector2(0, 0);

    float speed = 100;

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
        movePlanet((float)delta);
    }

    private void movePlanet(float delta)
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

        float distanceToOrigin = distanceBetweenPoints(tempPosition, origin);

        float currentM = findM(tempPosition.X, tempPosition.Y);

        float negativeM = -(1 / currentM);

        float step = 20f;

        float perpendicularX = distanceSolveForX(step, negativeM);
        float perpendicularY = distanceSolveForY(step, perpendicularX);

        //GD.Print("perX: " + perpendicularX + " perpY: " + perpendicularY);

        tempPosition.X += positiveOrNegative.X * Math.Abs(perpendicularX);
        tempPosition.Y += positiveOrNegative.Y * Math.Abs(perpendicularY);

        float afterStepXNoP = tempPosition.X < 0 ? -1 : 1;
        float afterStepYNoP = tempPosition.Y < 0 ? -1 : 1;

        float proposedM = findM(tempPosition.X, tempPosition.Y);
        //GD.Print("propM: " + proposedM);

        //GD.Print(
        //    "tempX + perpX: "
        //        + (tempPosition.X + (positiveOrNegative.X * perpendicularX))
        //        + " tempY + perpY: "
        //        + (tempPosition.Y + (positiveOrNegative.Y * perpendicularY))
        //);

        float proposedX = afterStepXNoP * distanceSolveForX(DistanceToParent, proposedM);
        float proposedY = afterStepYNoP * distanceSolveForY(DistanceToParent, proposedX);

        //GD.Print("propX: " + proposedX + " propY: " + proposedY);

        Position = new Godot.Vector2(proposedX, proposedY);

        //float perpendicularLineCross = solveForCrossingTheLine(tempPosition.X, tempPosition.Y, negativeM);
        //
        // float mStepForX =
        //     positiveOrNegative.X
        //     * Math.Abs((DistanceToParent - Math.Abs(tempPosition.X)) / DistanceToParent);
        // float mStepForY =
        //     positiveOrNegative.Y
        //     * Math.Abs((DistanceToParent - Math.Abs(tempPosition.Y)) / DistanceToParent);
        //
        // GD.Print("mStepForY: " + mStepForY + "positiveOrNegative.Y: " + positiveOrNegative.Y);
        //
        // Godot.Vector2 addStep = new Godot.Vector2(mStepForX, mStepForY);
        //
        // Godot.Vector2 afterStep = tempPosition + (addStep * delta * speed);
        //
        // float afterStepXNoP = afterStep.X < 0 ? -1 : 1;
        // float afterStepYNoP = afterStep.Y < 0 ? -1 : 1;
        //
        // GD.Print("afterStep: " + afterStep);
        // float mValueForX = (afterStep.Y != 0 && afterStep.X != 0) ? afterStep.Y / afterStep.X : 0f;
        // GD.Print("mValueX: " + mValueForX);
        // float calcX = (float)
        //     Math.Sqrt(Math.Pow(DistanceToParent, 2) / (Math.Pow(mValueForX, 2) + 1));
        // GD.Print("CalcX: " + calcX);
        // float calcY = calcX * Math.Abs(mValueForX);
        // GD.Print("CalcY: " + calcY);
        // Position = new Godot.Vector2(calcX * afterStepXNoP, calcY * afterStepYNoP);
        // GD.Print("Position: " + Position);
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
