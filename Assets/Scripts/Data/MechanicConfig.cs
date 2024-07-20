using Newtonsoft.Json;
using UnityEngine;

public class MechanicConfig
{
    public StackConfig StackConfig = null;
    public LevelConfig LevelConfig = null;
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

public class LevelConfig
{
    [JsonProperty]
    public LevelPresenterData PresenterData { get; private set; }
    [JsonProperty]
    public int[] SpaceClampf { get; private set; } = new int[2] { 20, 10 }; //Space Clampf Random Of Level EX: CurrentLevel 60 => Randomfrom [60 - SpaceClampf[0], 60 - SpaceClampf[1]];

    public LevelConfig(LevelPresenterData presenterData, int[] spaceClampf)
    {
        PresenterData = presenterData;
        SpaceClampf = spaceClampf;
    }
}
