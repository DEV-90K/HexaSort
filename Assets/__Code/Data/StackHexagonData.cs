using Newtonsoft.Json;

public class StackHexagonData
{
    //public HexagonData[] Hexagons { get; private set; } 
    [JsonProperty]
    public int[] IDHexes { get; set; }

    public StackHexagonData(int[] hexColors)
    {
        IDHexes = hexColors;
    }

    public StackHexagonData()
    {
    }

    public void SetIDHexas(int[] IDHex)
    {
        this.IDHexes = IDHex;
    }
}