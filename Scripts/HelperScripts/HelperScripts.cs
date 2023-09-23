using Godot;
using System.Collections.Generic;

public partial class HelperScripts
{
    Godot.Vector2 storedVelocity = new Godot.Vector2(0, 0);

    public void printVelocityChange(Godot.Vector2 velocity)
    {
        if (storedVelocity != velocity)
        {
            GD.Print(velocity);
            storedVelocity = velocity;
        }
    }

    public void printDictionaryValues(Dictionary<string, string> valueDictionary)
    {
        string finalString = "";

        foreach (KeyValuePair<string, string> kVP in valueDictionary)
        {
            finalString += $"{kVP.Key}: {kVP.Value}";
        }

        GD.Print(finalString);
    }
}
