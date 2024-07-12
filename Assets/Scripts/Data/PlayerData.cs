using Newtonsoft.Json;
public class PlayerData
{
    public int Coin { get; set; }

    [JsonProperty]
    public PlayerLevelData PlayerLevel { get; private set; }

    public PlayerData(int coin, PlayerLevelData playerLevel)
    {
        Coin = coin;
        PlayerLevel = playerLevel;
    }
}

public class PlayerLevelData
{
    public int IDLevel { get; private set; } // Max Level Already Playing
    public LevelData Level { get; private set; } //Data Of Current Player Playing

    public PlayerLevelData(int iDLevel, LevelData level)
    {
        IDLevel = iDLevel;
        Level = level;
    }

    public void UpdateIDLevel(int id)
    { 
        this.IDLevel = id;
    }

    public void UpdateLevelData(LevelData level)
    {
        this.Level = level.CopyObject();
    }
}
