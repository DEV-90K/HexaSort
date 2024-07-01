using Newtonsoft.Json;

public class GridHexagonData
{
    [JsonProperty]
    public GridHexagonState State; //NONE, UNLOCK
    [JsonProperty]
    public int Row { get; private set; }
    [JsonProperty]
    public int Column { get; private set; }

    [JsonProperty]
    public string HexColor { get; private set; }

    [JsonProperty]
    public StackHexagonData StackHexagon { get; private set; }

    public GridHexagonData(GridHexagonState state, int row, int column, string hexColor, StackHexagonData stackHexagon)
    {
        State = state;
        Row = row;
        Column = column;
        HexColor = hexColor;
        StackHexagon = stackHexagon;
    }

    public GridHexagonData()
    {
    }
}

public class GridHexagonLockData
{
    public int Goal {  get; private set; }
    public string HexColor { get; private set; }
    public GridHexagonData GridHexagonData { get; private set; }

    public GridHexagonLockData(int goal, string hexColor, GridHexagonData gridHexagonData)
    {
        Goal = goal;
        HexColor = hexColor;
        GridHexagonData = gridHexagonData;
    }

    public GridHexagonLockData()
    {
    }
}

public enum GridHexagonState
{
    NONE,
    UNLOCK,
    LOCK_BY_GOAL,
    LOCK_BY_ADS
}
