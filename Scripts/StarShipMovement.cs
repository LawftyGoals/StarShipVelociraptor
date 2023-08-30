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

    private float stoppingPoint = 6f;

    public float MaxShipVelocity { get; set; } = 80f;

    [Export]
    public float ShipAcceleration { get; set; } = 2f;

    [Export]
    public float RotationSpeed { get; set; } = 5f;

    [Export]
    private float momentum = 0f;

    private float _rotationDirection;

    CollisionPolygon2D _childCollisionPolygon;

    public override void _Ready()
    {
        _childCollisionPolygon = GetChild<CollisionPolygon2D>(0);
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        RotateShip(delta);
        MoveAndSlide();
        //velocityPrint.printVelocityChange(Velocity);
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

    private Godot.Vector2 directionValue()
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

    private void SetShipVelocity()
    {
        if (Input.IsKeyPressed(Key.W))
        {
            momentum += 0.5f;

            GD.Print(directionValue() * ShipAcceleration);
            Velocity += directionValue() * ShipAcceleration;
        }
        else if (Input.IsKeyPressed(Key.S))
        {
            momentum -= 0.5f;
            Velocity -= directionValue() * ShipAcceleration;
        }

        if (Math.Sqrt(Math.Pow(Velocity.X, 2) + Math.Pow(Velocity.Y, 2)) >= 80)
            ;
        Velocity = new Godot.Vector2(
            Math.Clamp((float)Velocity.X, -(float)MaxShipVelocity, (float)MaxShipVelocity),
            Math.Clamp((float)Velocity.Y, -(float)MaxShipVelocity, (float)MaxShipVelocity)
        );

        if (
            (
                Velocity.X < stoppingPoint
                && Velocity.X > -stoppingPoint
                && Velocity.Y < stoppingPoint
                && Velocity.Y > -stoppingPoint
            ) && !(Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.S))
        )
            Velocity = new Godot.Vector2(0, 0);

        //speedLimiterToMax();
    }

    public void GetInput()
    {
        GetShipRotation();
        SetShipVelocity();
    }

    private void speedLimiterToMax()
    {
        /** y = mx
        // D = Math.Sqrt(Math.Pow(x,2) + Math.Pow(y,2))
        // rX = real X
        rY = real Y
        cX = clamped X
        cY = clamped Y
        cD = clamped Distance (80)

        m = rY/rX

        cD = Math.Sqrt(Math.Pow(cX,2) + Math.Pow(cX*m,2))

        cX = Math.Sqrt(Math.Pow(cD,2)/(2*Math.Pow(m,2)))

        cY = cXm


        **/

        float realDistanceVector = (float)
            Math.Sqrt(Math.Pow(Velocity.X, 2) + Math.Pow(Velocity.Y, 2));

        if (realDistanceVector > MaxShipVelocity)
        {
            float rY = Velocity.Y;
            float rX = Velocity.X;
            GD.Print("realDistanceVector: " + realDistanceVector);
            GD.Print("velocity: " + Velocity);

            GD.Print("rX: " + rX + " rY: " + rY);

            float m = rX == 0 ? 0 : rY / rX;

            float denominatorPower = m > 0 || m < 0 ? (2 * (float)Math.Pow(m, 2)) : 1;

            float cX = (float)Math.Sqrt(Math.Pow(MaxShipVelocity, 2) / denominatorPower);

            float cY = cX * m;
            GD.Print("m: " + m);
            GD.Print("cX: " + cX + " cY: " + cY);

            if (rY < 0)
            {
                cY = cY * -1;
            }

            if (rX < 0)
            {
                cX = cX * -1;
            }

            Velocity = new Godot.Vector2(cX, cY);
        }
    }
}
