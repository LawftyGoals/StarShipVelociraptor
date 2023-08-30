using Godot;
using System;
using System.ComponentModel;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using static HelperScripts;

public partial class StarShipMovement : CharacterBody2D
{
    private HelperScripts velocityPrint = new HelperScripts();
    public Godot.Vector2 compare = new Godot.Vector2(0, 0);

    private float stoppingPoint = 6f;

    public float MaxShipVelocity { get; set; } = 80f;

    [Export]
    public float ShipAcceleration { get; set; } = 2f;

    [Export]
    public float RotationSpeed { get; set; } = 5f;

    [Export]
    private float momentum = 0f;

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
        velocityPrint.printVelocityChange(Velocity);
    }

    public void RotateShip(double delta)
    {
        _childCollisionPolygon.GlobalRotation += _rotationDirection * RotationSpeed * (float)delta;
    }

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

    private float positiveChildGlobalRotation()
    {
        return (float)Math.Sqrt(Math.Pow(_childCollisionPolygon.GlobalRotation, 2f));
    }

    private int positiveOrNegative()
    {
        if (_childCollisionPolygon.GlobalRotation >= 0 && positiveChildGlobalRotation() >= 0)
        {
            return 1;
        }

        return -1;
    }

    public Godot.Vector2 directionValue()
    {
        return (
            new Godot.Vector2(
                positiveOrNegative()
                    * (
                        1
                        - (
                            (float)Math.Sqrt(Math.Pow(positiveChildGlobalRotation() - 1.5708f, 2f))
                            / 1.5708f
                        )
                    ),
                -(1.5708f - positiveChildGlobalRotation()) / 1.5708f
            )
        );
    }

    private void SetShipVelocity(double delta)
    {
        Godot.Vector2 tempVelocity = Velocity;

        if (Input.IsKeyPressed(Key.W))
        {
            float accelAndDelta = ShipAcceleration * (float)delta * 100;
            tempVelocity += directionValue() * ShipAcceleration;
            //GD.Print(Math.Sqrt(Math.Pow(tempVelocity.X, 2) + Math.Pow(tempVelocity.Y, 2)));
        }
        else if (Input.IsKeyPressed(Key.S))
        {
            tempVelocity -= directionValue() * ShipAcceleration;
        }

        if (Math.Sqrt(Math.Pow(tempVelocity.X, 2) + Math.Pow(tempVelocity.Y, 2)) > 80)
        {
            Godot.Vector2 comparison = tempVelocity - Velocity;
            compare = comparison;

            tempVelocity = new Godot.Vector2(
                Math.Clamp(tempVelocity.X, -distanceVectorMaxSpeed().X, distanceVectorMaxSpeed().X),
                Math.Clamp(tempVelocity.Y, -distanceVectorMaxSpeed().Y, distanceVectorMaxSpeed().Y)
            );
        }

        if (
            (
                tempVelocity.X < stoppingPoint
                && tempVelocity.X > -stoppingPoint
                && tempVelocity.Y < stoppingPoint
                && tempVelocity.Y > -stoppingPoint
            ) && !(Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.S))
        )
            tempVelocity = new Godot.Vector2(0, 0);

        Velocity = tempVelocity;
    }

    public void GetInput(double delta)
    {
        GetShipRotation();
        SetShipVelocity(delta);
    }

    private Godot.Vector2 distanceVectorMaxSpeed()
    {
        float rY = Math.Abs(Velocity.Y);
        float rX = Math.Abs(Velocity.X);

        float cX;
        float cY;
        GD.Print("realDistanceVector: ");
        GD.Print("velocity: " + Velocity);

        GD.Print("rX: " + rX + " rY: " + rY);
        if (rX <= rY)
        {
            float m = rX == 0 ? 0 : rY / rX;

            float denominatorPower = m > 0 || m < 0 ? (2 * (float)Math.Pow(m, 2)) : 1;

            cX = (float)Math.Sqrt(Math.Pow(MaxShipVelocity, 2) / denominatorPower);
            GD.Print("calc: " + cX);
            cY = cX * m;
            GD.Print("m: " + m);
            GD.Print("cX: " + cX + " cY: " + cY);
        }
        else
        {
            float m = rY == 0 ? 0 : rX / rY;

            float denominatorPower = m > 0 || m < 0 ? (2 * (float)Math.Pow(m, 2)) : 1;

            cY = (float)Math.Sqrt(Math.Pow(MaxShipVelocity, 2) / denominatorPower);
            GD.Print("calc: " + cY);
            cX = cY * m;
            GD.Print("m: " + m);
            GD.Print("cX: " + cX + " cY: " + cY);
        }

        return new Godot.Vector2(cX, cY);
    }
}
