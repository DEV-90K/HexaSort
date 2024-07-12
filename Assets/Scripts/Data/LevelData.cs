using Newtonsoft.Json;

public class LevelData
{
    [JsonProperty]
    public GridData Grid { get; private set; }

    [JsonProperty]
    public StackQueueData StackQueueData { get; private set; }

    public LevelData(GridData grid, StackQueueData stackQueueData)
    {
        Grid = grid;
        StackQueueData = stackQueueData;
    }

    public LevelData(GridData grid)
    {
        Grid = grid;
        StackQueueData = null;
    }

    public LevelData()
    {
    }

    public void UpdateGridData(GridData grid)
    {
        Grid = grid;
    }
}

public class LevelPresenterData
{
    [JsonProperty]
    public int Level { get; private set; }
    [JsonProperty]
    public int Goal { get; private set; }

    public LevelPresenterData()
    {
    }

    public LevelPresenterData(int level, int goal)
    {
        Level = level;
        Goal = goal;
    }

    public void UpdateLevel(int level)
    {
        Level = level;
    }

    public void UpdateGoal(int goal)
    {
        Goal = goal;
    }
}

