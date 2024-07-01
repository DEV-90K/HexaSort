using Newtonsoft.Json;
using System.Data;

public class HexagonData
{
    public HexagonData()
    {
    }
    [JsonProperty]
    public int ID { get; private set; }
    [JsonProperty]
    public string HexColor { get; private set; }

    public HexagonData(int id, string hexColor)
    {
        ID = id;
        HexColor = hexColor;
    }
}