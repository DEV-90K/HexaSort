using Newtonsoft.Json;
using UnityEngine;

public enum VanishType
{
    NONE,
    RANDOM,
    AROUND,
    CONTAIN_COLOR
}

public class StackVanishData
{
    public VanishType Type;
    public string Name;
    public string Description;
}
