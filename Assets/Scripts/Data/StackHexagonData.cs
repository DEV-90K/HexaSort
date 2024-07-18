using Newtonsoft.Json;
using System;

public class StackHexagonData
{
    [JsonProperty]
    public int[] IDHexes {  get; private set; }

    public StackHexagonData(int[] hexColors)
    {
        IDHexes = hexColors;
    }

    public StackHexagonData()
    {
    }

    public void UpdateIDHexs(int[] idHexs)
    {
        IDHexes = idHexs;
    }
}
