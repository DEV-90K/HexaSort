public class PlayerData
{
    public int Coin { get; set; }
    public int Material { get; set; }
    public int Hammer = 23;
    public int Swap = 24;
    public int Refresh = 1054;
    public string ChestLastTime;
    public PlayerLevelData PlayerLevel;
    public PlayerAudioData PlayerAudio;
    public PlayerData(int coin, int material, PlayerLevelData playerLevel)
    {
        Coin = coin;
        Material = material;
        PlayerLevel = playerLevel;
    }
}

public class PlayerLevelData
{
    public int IDLevel { get; private set; } // Max Level Already Playing
    public int AmountCollected { get; private set; } = 0;
    public LevelData Level { get; private set; } //Data Of Current Player Playing
    public LevelPresenterData LevelPresenter { get; private set; }
    public PlayerLevelData(int iDLevel, LevelData level, LevelPresenterData levelPresenter)
    {
        IDLevel = iDLevel;
        Level = level;
        LevelPresenter = levelPresenter;
    }

    public void UpdateIDLevel(int id)
    { 
        this.IDLevel = id;
    }

    public void UpdateLevelData(LevelData level)
    {
        this.Level = level.CopyObject();
    }

    public void UpdateLevelPresenterData(LevelPresenterData levelPresenter)
    {
        this.LevelPresenter = levelPresenter;
    }

    public void UpdateAmountCollected(int amount)
    {
        this.AmountCollected = amount;
    }
}

public class PlayerAudioData
{
    public float SoundVol;
    public bool CanSound;

    public float MusicVol;
    public bool CanMusic;

}