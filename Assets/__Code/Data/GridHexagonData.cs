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
    public int IDHex { get; private set; }

    [JsonProperty]
    public int IDHexLock { get; private set; }

    [JsonProperty]
    public StackHexagonData StackHexagon { get; private set; }

    public GridHexagonData(GridHexagonState state, int row, int column, int idHex, StackHexagonData stackHexagon)
    {
        State = state;
        Row = row;
        Column = column;
        IDHex = idHex;
        IDHexLock = IDHexLock;
        StackHexagon = stackHexagon;
    }

    public GridHexagonData(GridHexagonState state, int row, int column, int iDHex, int iDHexLock, StackHexagonData stackHexagon)
    {
        State = state;
        Row = row;
        Column = column;
        IDHex = iDHex;
        IDHexLock = iDHexLock;
        StackHexagon = stackHexagon;
    }

    public GridHexagonData()
    {
    }

    public void SetStackHexagonData(StackHexagonData stackHexagon)
    {
        this.StackHexagon = stackHexagon;
    }

    public void UpdateStackHexagonData(StackHexagonData stackData)
    {
        StackHexagon = stackData;
    }
}

public class GridHexagonLockData
{
    public int Goal { get; private set; }
    [JsonProperty]
    public int IDHex { get; private set; }
    public GridHexagonData GridHexagonData { get; private set; }

    public GridHexagonLockData(int goal, int idHex, GridHexagonData gridHexagonData)
    {
        Goal = goal;
        IDHex = idHex;
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