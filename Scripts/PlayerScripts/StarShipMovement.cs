using Godot;
using Microsoft.Win32.SafeHandles;
using System;
using static HelperScripts;

public partial class StarShipMovement : CharacterBody2D
{
    //private HelperScripts velocityPrint = new HelperScripts();
    private float stoppingPoint = 6f;

    public float MaxShipVelocity { get; set; } = 200f;

    [Export]
    public float ShipAcceleration { get; set; } = 100f;

    [Export]
    public float RotationSpeed { get; set; } = 5f;
    private float _rotationDirection;

    public CollisionPolygon2D _childCollisionPolygon;

    public override void _Ready()
    {
        _childCollisionPolygon = GetChild<CollisionPolygon2D>(0);
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput(delta);
        RotateShip(delta);
        MoveAndSlide();
    }

    public void GetInput(double delta)
    {
        GetShipRotation();
        SetShipVelocity(delta);
    }

    public void RotateShip(double delta) =>
        _childCollisionPolygon.GlobalRotation += _rotationDirection * RotationSpeed * (float)delta;

    private void GetShipRotation()
    {
        if (Input.IsKeyPressed(Key.A))
        {
            _rotationDirection = -1;
        }
        else if (Input.IsKeyPressed(Key.D))
        {
            _rotationDirection = 1;
        }
        else
        {
            _rotationDirection = 0;
        }
    }

    private int positiveOrNegative()
    {
        if (_childCollisionPolygon.GlobalRotation >= 0 && positiveChildGlobalRotation() >= 0)
        {
            return 1;
        }

        return -1;
    }

    private float positiveChildGlobalRotation() =>
        (float)Math.Round(Math.Abs(_childCollisionPolygon.GlobalRotation), 4);

    public Godot.Vector2 directionValue() =>
        new Godot.Vector2(
            positiveOrNegative()
                * (1 - ((float)Math.Abs(positiveChildGlobalRotation() - 1.5708f) / 1.5708f)),
            -(1.5708f - positiveChildGlobalRotation()) / 1.5708f
        );

    public Godot.Vector2 directionValueStrafe() =>
        new Godot.Vector2(
            -(1.5708f - positiveChildGlobalRotation()) / 1.5708f,
            -positiveOrNegative()
                * (1 - ((float)Math.Abs(positiveChildGlobalRotation() - 1.5708f) / 1.5708f))
        );

    private void SetShipVelocity(double delta)
    {
        Godot.Vector2 tempVelocity = Velocity;

        float deltaAcceleration = ShipAcceleration * (float)delta;

        if (Input.IsKeyPressed(Key.W))
        {
            tempVelocity += directionValue() * deltaAcceleration;
        }

        if (Input.IsKeyPressed(Key.S))
        {
            tempVelocity -= directionValue() * deltaAcceleration;
        }

        if (Input.IsKeyPressed(Key.Q))
        {
            tempVelocity += directionValueStrafe() * deltaAcceleration;
        }

        if (Input.IsKeyPressed(Key.E))
        {
            tempVelocity -= directionValueStrafe() * deltaAcceleration;
        }

        if (Math.Sqrt(Math.Pow(tempVelocity.X, 2) + Math.Pow(tempVelocity.Y, 2)) > MaxShipVelocity)
        {
            tempVelocity = new Godot.Vector2(
                Math.Clamp(
                    tempVelocity.X,
                    -distanceVectorMaxSpeed(tempVelocity).X,
                    distanceVectorMaxSpeed(tempVelocity).X
                ),
                Math.Clamp(
                    tempVelocity.Y,
                    -distanceVectorMaxSpeed(tempVelocity).Y,
                    distanceVectorMaxSpeed(tempVelocity).Y
                )
            );
        }

        if (
            (
                tempVelocity.X > -stoppingPoint
                && tempVelocity.X < stoppingPoint
                && tempVelocity.Y > -stoppingPoint
                && tempVelocity.Y < stoppingPoint
            )
            && !(
                Input.IsKeyPressed(Key.W)
                || Input.IsKeyPressed(Key.S)
                || Input.IsKeyPressed(Key.Q)
                || Input.IsKeyPressed(Key.E)
            )
        )
            tempVelocity = new Godot.Vector2(0, 0);

        Velocity = tempVelocity;
    }

    private Godot.Vector2 distanceVectorMaxSpeed(Godot.Vector2 tempVelocity)
    {
        float rY = Math.Abs(tempVelocity.Y);
        float rX = Math.Abs(tempVelocity.X);

        float cX;
        float cY;

        if (rX >= rY)
        {
            float m = rX == 0 ? 0 : rY / rX;

            float denominatorPower = (float)Math.Pow(m, 2) + 1;

            cX = (float)Math.Sqrt(Math.Pow(MaxShipVelocity, 2) / denominatorPower);

            cY = cX * m;
        }
        else
        {
            float m = rY == 0 ? 0 : rX / rY;

            float denominatorPower = (float)Math.Pow(m, 2) + 1;

            cY = (float)Math.Sqrt(Math.Pow(MaxShipVelocity, 2) / denominatorPower + 1);

            cX = cY * m;
        }

        return new Godot.Vector2(cX, cY);
    }
}
