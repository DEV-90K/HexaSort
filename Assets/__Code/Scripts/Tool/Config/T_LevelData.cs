using UnityEngine;

public class T_LevelData
{
    public int Level;
    public int NumberHexaInEachHexa;
    public int NumberColor;
}

public class T_HexaInBoardData
{
    public int Id {  get; set; }
    public bool IsSelected { get; set; }
    public string ColorHexa { get; set; }
    public T_HexaInBoardData[] HexagonDatas { get; set; }
}

