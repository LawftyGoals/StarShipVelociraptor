using Godot;
using System;

public partial class UnusedCode : Node
{

    //PRIMARY ATTEMPT AT CIRCULAR ORBIT THOUGH DID NOT FUNCTION BECAUSE SLOWED DOWN AT AXIS
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
