using Newtonsoft.Json;

public class StackHexagonData
{
    //public HexagonData[] Hexagons { get; private set; } 
    [JsonProperty]
    public int[] IDHexes {  get; private set; }

    public StackHexagonData(int[] hexColors)
    {
        IDHexes = hexColors;
    }

    public StackHexagonData()
    {
    }
}
