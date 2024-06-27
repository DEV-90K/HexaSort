public class GridHexagonData
{
    public GridHexagonState State; //NONE, UNLOCK
    public int Row { get; private set; }
    public int Column { get; private set; }

    //public HexagonData Hexagon { get; private set; } //Prototype
    public string HexColor { get; private set; }
    public StackHexagonData StackHexagon { get; private set; }
    
}

public class GridHexagonLockData
{
    public int Goal {  get; private set; }
    public string HexColor { get; private set; }
    public GridHexagonData GridHexagonData { get; private set; }
}

public enum GridHexagonState
{
    NONE,
    UNLOCK,
    LOCK_BY_GOAL,
    LOCK_BY_ADS
}
