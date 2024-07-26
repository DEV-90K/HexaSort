using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public string Name;
    [TextArea(3, 10)]
    public string[] Sentences;
}
