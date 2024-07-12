using Newtonsoft.Json;
using UnityEngine;

public class MechanicConfig
{
    public StackConfig StackConfig;
}

public class StackConfig
{
    [JsonProperty]
    public int[] AmountClampf { get; private set; } = new int[2] { 5, 5 }; //Amount of Hexagon in stack [val1, val2)
    [JsonProperty]
    public int NumberOfColor { get; private set; } = 3; // Max Number Of Color In Stack

    public StackConfig(int[] AmountClampf, int NumberOfColor)
    {
        this.AmountClampf = AmountClampf;
        this.NumberOfColor = NumberOfColor;
    }
}
