using Newtonsoft.Json;

public class LevelData
{
    public int ID { get; private set; }
    public int IDGrid { get; private set; }
    public int Goal { get; private set; }

    [JsonIgnore]
    public GridData Grid { get; private set; }
}

