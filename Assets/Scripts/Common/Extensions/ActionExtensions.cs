using System;

public static class ActionExtension
{
    public static void RemoveAllEvents(this Action parent)
    {
        foreach (Delegate deg in parent.GetInvocationList())
        {
            parent -= (Action)deg;
        }
    }
}
