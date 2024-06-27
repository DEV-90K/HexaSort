public class PlayerData
{
    public PlayerLevelData PlayerLevel { get; set; }
}

public class PlayerLevelData
{
    public int IDLevel { get; private set; } // Max Level Already Playing
    public LevelData Level { get; private set; } //Data Of Current Player Playing
}
