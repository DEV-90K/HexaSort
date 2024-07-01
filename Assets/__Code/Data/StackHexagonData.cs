using Newtonsoft.Json;

public class StackHexagonData
{
    //public HexagonData[] Hexagons { get; private set; } 
    [JsonProperty]
    public string[] HexColors {  get; private set; }

    public StackHexagonData(string[] hexColors)
    {
        HexColors = hexColors;
    }

    public StackHexagonData()
    {
    }
}
