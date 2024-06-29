using Newtonsoft.Json;

public class LevelData
{
    [JsonProperty]
    public GridData Grid { get; private set; }

    public LevelData(GridData grid)
    {
        Grid = grid;
    }

    public LevelData()
    {
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
}

