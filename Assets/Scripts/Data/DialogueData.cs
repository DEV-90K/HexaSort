using UnityEngine;

public enum DialogueType
{
    NONE,
    WELCOME,
    CHEST_REWARD,
    RELIC_COLLECT
}

[System.Serializable]
public class DialogueData
{
    public DialogueType Type;
    public string Name;
    [TextArea(3, 10)]
    public string[] Sentences;
}
