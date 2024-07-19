using System;

public static class EnumUtils
{
    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
    
    public static string ParseString(Enum value) 
    {
        return Enum.GetName(value.GetType(), value);
    }
}