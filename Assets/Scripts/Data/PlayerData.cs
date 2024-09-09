using System.Collections.Generic;

public class PlayerData
{
    public int Coin { get; set; }
    public int Material { get; set; }
    public int Hammer { get; set; } = 23;
    public int Swap { get; set; } = 24;
    public int Refresh { get; set; } = 1054;
    public string ChestLastTime { get; set; }
    public PlayerLevelData PlayerLevel { get; set; }
    public PlayerAudioData PlayerAudio { get; set; }
    public Dictionary<int, List<GalleryRelicData>> DictGalleryRelic { get; set; }
    public PlayerData(int coin, int material, PlayerLevelData playerLevel)
    {
        Coin = coin;
        Material = material;
        PlayerLevel = playerLevel;
    }

    public PlayerData()
    {
        this.PlayerLevel = new PlayerLevelData();
        this.PlayerAudio = new PlayerAudioData();
        this.DictGalleryRelic = new Dictionary<int, List<GalleryRelicData>>();
    }
}

public class PlayerLevelData
{
    public int IDLevel { get;  set; } // Max Level Already Playing
    public int AmountCollected { get;  set; }
    public LevelData Level { get;  set; } //Data Of Current Player Playing
    public LevelPresenterData LevelPresenter { get;  set; }
    public PlayerLevelData(int iDLevel, LevelData level, LevelPresenterData levelPresenter)
    {
        IDLevel = iDLevel;
        Level = level;
        LevelPresenter = levelPresenter;
    }

    public PlayerLevelData()
    {

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