using Newtonsoft.Json;

public class StackQueueData
{
    [JsonProperty]
    public StackHexagonData[] StackHexagonDatas { get; private set; }

    public StackQueueData()
    {
    }

    public StackQueueData(StackHexagonData[] stackHexagonDatas)
    {
        this.StackHexagonDatas = stackHexagonDatas;
    }
}
