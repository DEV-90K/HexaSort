using Newtonsoft.Json;
using UnityEngine;

public enum VisualState
{
    SHOW, 
    HIDE
}
public class T_LevelData
{
    public int Level;
    public T_HexaInBoardData[] HexaInBoardDatas;

    /*[JsonIgnore] // Bỏ qua thuộc tính này khi export ra file Json;
    public int HexaNumberInEachHexa;
    [JsonIgnore] // Bỏ qua thuộc tính này khi export ra file Json;
    public int ColorNumber;*/
}

public class T_HexaInBoardData
{
    public int Id {  get; set; }
    public bool IsSelected { get; set; }
    public string ColorHexa { get; set; }
    public VisualState State { get; set; }
    public T_HexaInBoardData[] HexagonDatas { get; set; }
}

