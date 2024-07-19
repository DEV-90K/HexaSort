using Newtonsoft.Json;

public class GridData
{
    [JsonProperty]
    public GridHexagonData[] GridHexagonDatas { get; private set; }

    public GridData(GridHexagonData[] gridHexagonDatas)
    {
        GridHexagonDatas = gridHexagonDatas;
    }

    public GridData()
    {
    }

    public void UpdateGridHexagonDatas(GridHexagonData[] datas)
    {
        GridHexagonDatas = datas;
    }
}
