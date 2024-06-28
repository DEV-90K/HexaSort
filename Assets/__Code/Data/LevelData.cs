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
    public int Level { get; private set; }
    public int Goal { get; private set; }
}

