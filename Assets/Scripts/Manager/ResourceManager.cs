using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : PersistentMonoSingleton<ResourceManager>
{
    private const int TEST_IDLEVEL = 1;

    private MechanicConfig _mechanicConfig;
    
    private Dictionary<int, LevelData> _levelDataDict = new Dictionary<int, LevelData>();

    private LevelPresenterData[] _levelPresenterDatas;
    private Dictionary<int, LevelPresenterData> _levelPresenterDatasDict = new Dictionary<int, LevelPresenterData>();

    private Dictionary<int, ChallengeData> _challengeDataDict = new Dictionary<int, ChallengeData>();
    
    private ChallengePresenterData[] _challengePresenterDatas;
    private Dictionary<int, ChallengePresenterData> _challengePresenterDataDict = new Dictionary<int, ChallengePresenterData>();

    private HexagonData[] _hexagonDatas;
    private Dictionary<int, HexagonData> _cacheHexagonData = new Dictionary<int, HexagonData>();

    private Dictionary<string, Sprite> _cacheRelicSprite = new Dictionary<string, Sprite>();
    private RelicData[] _relicDatas;

    private GalleryData[] _galleryDatas;

    protected override void Awake()
    {
        base.Awake();
        this.LoadResource();
    }

    public void LoadResource()
    {
        _levelPresenterDatas = LoadLevelPresenterDatas();
        _challengePresenterDatas = LoadChallengePresenterDatas();

        _hexagonDatas = LoadHexagonData();
        _mechanicConfig = LoadMechanicConfig();

        //TEST
        _relicDatas = createRelicData();
        _galleryDatas = CreateGalleryDatas();
        _galleryDatas.DebugLogObject();
    }

    #region Mechanic Config

    public StackConfig GetStackConfig()
    {
        return _mechanicConfig.StackConfig;
    }

    private MechanicConfig LoadMechanicConfig()
    {
        MechanicConfig config = null;//FirebaseManager.instance.LoadRemoteMechanicConfig();

        if(config == null)
        {
            Debug.Log("Load Local");
            config = LoadLocalMechanicConfig();
        }

        if(config == null)
        {
            Debug.Log("Create New");
            config = CreateMechanicConfig();            
        }

        config.DebugLogObject();

        return config;
    }

    private MechanicConfig CreateMechanicConfig()
    {
        MechanicConfig config = new MechanicConfig();
        config.StackConfig = new StackConfig(new int[] {6, 8}, 3);
        
        return config;
    }

    private MechanicConfig LoadLocalMechanicConfig()
    {
        string key = "Mechanic";
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format("Config/{0}", key));
        if (textAsset != null)
        {
            return JsonConvert.DeserializeObject<MechanicConfig>(textAsset.text.Trim());
        }
        return null;
    }
    #endregion Mechanic Config

    #region Level Data
    public LevelData GetLevelByID(int IDLevel = TEST_IDLEVEL)
    {
        if(_levelDataDict.ContainsKey(IDLevel))
        {
            return _levelDataDict[IDLevel];
        }

        LevelData levelData = LoadLevelData(IDLevel);

        if(levelData == null)
        {
            levelData = GetLevelDataByRandom(IDLevel);
        }

        _levelDataDict[IDLevel] = levelData;

        return levelData;
    }

    private LevelData GetLevelDataByRandom(int IDMax)
    {
        int IDLevel = UnityEngine.Random.Range(1, IDMax);

        if (_levelDataDict.ContainsKey(IDLevel))
        {
            return _levelDataDict[IDLevel].CopyObject();
        }

        return LoadLevelData(IDMax).CopyObject();
    }

    private LevelData LoadLevelData(int IDLevel)
    {
        LevelData levelData = null; //FirebaseManager.instance.GetRemoteLevelData(IDLevel);

        if(levelData == null)
        {
            levelData = LoadLocalLevelData(IDLevel);
        }

        return levelData;
    }

    private LevelData LoadLocalLevelData(int IDLevel)
    {
        string key = "Level_" + IDLevel;
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format("Config/Data/Levels/{0}", key));
        if (textAsset != null)
        {
            return JsonConvert.DeserializeObject<LevelData>(textAsset.text.Trim());
        }
        return null;
    }
    #endregion Level Data

    #region Level Presenter Data
    public LevelPresenterData GetLevelPresenterDataByID(int IDLevel)
    {
        if(_levelPresenterDatasDict.ContainsKey(IDLevel))
        {
            return (_levelPresenterDatasDict[IDLevel]);
        }

        LevelPresenterData levelPresenterData = null;

        for (int i = 0; i < _levelPresenterDatas.Length; i++)
        {
            if (_levelPresenterDatas[i].Level == IDLevel)
            {
                levelPresenterData = _levelPresenterDatas[i];
                break;
            }
        }

        if(levelPresenterData == null)
        {
            levelPresenterData = GetLevelPresenterDataByRandom();
        }

        levelPresenterData.UpdateLevel(IDLevel);
        _levelPresenterDatasDict[IDLevel] = levelPresenterData;

        return levelPresenterData;
    }

    private LevelPresenterData GetLevelPresenterDataByRandom()
    {
        int IDXLevel = UnityEngine.Random.Range(0, _levelPresenterDatas.Length);
        return _levelPresenterDatas[IDXLevel].CopyObject();
    }

    private LevelPresenterData[] LoadLevelPresenterDatas()
    {
        LevelPresenterData[] presenterDatas = null; // FirebaseManager.instance.GetRemoteLevelPresenterDatas();

        if (presenterDatas == null)
        {
            presenterDatas = LoadLocalLevelPresenterDatas();
        }

        return presenterDatas;
    }

    private LevelPresenterData[] LoadLocalLevelPresenterDatas()
    {
        string key = "LevelPresenters";
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format("Config/Presenter/{0}", key));

        if (textAsset != null)
        {
            return JsonConvert.DeserializeObject<LevelPresenterData[]>(textAsset.text.Trim());
        }

        return null;
    }
    #endregion Level Presenter Data

    #region Challenge Data

    public ChallengeData GetChallengeByID(int IDChallenge = TEST_IDLEVEL)
    {
        if(_challengeDataDict.ContainsKey(IDChallenge))
        {
            return _challengeDataDict[IDChallenge];
        }

        ChallengeData challengeData = LoadChallengeData(IDChallenge);

        if(challengeData == null)
        {
            challengeData = GetChallengeDataByRandom(IDChallenge);
        }

        _challengeDataDict[IDChallenge] = challengeData;
        return challengeData;
    }

    private ChallengeData GetChallengeDataByRandom(int IDMax)
    {
        int IDChallenge = UnityEngine.Random.Range(1, IDMax);

        if (_challengeDataDict.ContainsKey(IDChallenge))
        {
            return _challengeDataDict[IDChallenge].CopyObject();
        }

        return LoadChallengeData(IDChallenge).CopyObject();
    }

    private ChallengeData LoadChallengeData(int IDChallenge)
    {
        ChallengeData challengeData = null; //FirebaseManager.instance.GetRemoteChallengeData(IDChallenge);

        if (challengeData == null)
        {
            challengeData = LoadLocalChallengeData(IDChallenge);
        }

        return challengeData;
    }

    private ChallengeData LoadLocalChallengeData(int IDChallenge)
    {
        string key = "Challenge_" + IDChallenge;
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format("Config/Data/Challenges/{0}", key));
        if (textAsset != null)
        {
            Debug.Log("Key: " + textAsset.text.Trim());
            return JsonConvert.DeserializeObject<ChallengeData>(textAsset.text.Trim());
        }
        return null;
    }

    #endregion Challenge Data

    #region Challenge Presenter Data
    public ChallengePresenterData GetChallengePresenterDataByID(int IDChallenge)
    {
        if (_challengePresenterDataDict.ContainsKey(IDChallenge))
        {
            return _challengePresenterDataDict[IDChallenge];
        }

        ChallengePresenterData challengePresenterData = null;

        for (int i = 0; i < _challengePresenterDatas.Length; i++)
        {
            if (_challengePresenterDatas[i].Challenge == IDChallenge)
            {
                challengePresenterData = _challengePresenterDatas[i];
                break;
            }
        }

        if (challengePresenterData == null)
        {
            challengePresenterData = GetChallengePresenterDataByRandom();
        }

        challengePresenterData.UpdateChallenge(IDChallenge);
        _challengePresenterDataDict[IDChallenge] = challengePresenterData;

        return challengePresenterData;
    }

    private ChallengePresenterData GetChallengePresenterDataByRandom()
    {
        int IDXChallenge = UnityEngine.Random.Range(0, _challengePresenterDatas.Length);
        return _challengePresenterDatas[IDXChallenge].CopyObject();
    }

    private ChallengePresenterData[] LoadChallengePresenterDatas()
    {
        ChallengePresenterData[] presenterDatas = null; //FirebaseManager.instance.GetRemoteChallengePresenterDatas();

        if (presenterDatas == null)
        {
            presenterDatas = LoadLocalChallengePresenterDatas();
        }

        if(presenterDatas == null)
        {   
            presenterDatas = CreateChallengePresenterDatas();
        }        
        
        return presenterDatas;
    }

    private ChallengePresenterData[] LoadLocalChallengePresenterDatas()
    {
        string key = "ChallengePresenters";
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format("Config/Presenter/{0}", key));

        if (textAsset != null)
        {
            return JsonConvert.DeserializeObject<ChallengePresenterData[]>(textAsset.text.Trim());
        }

        return null;
    }

    private ChallengePresenterData[] CreateChallengePresenterDatas()
    {
        List<ChallengePresenterData> challengePresenters = new List<ChallengePresenterData>();
        ChallengePresenterData challengePresentersData_1 = new ChallengePresenterData(1);
        ChallengePresenterData challengePresentersData_2 = new ChallengePresenterData(2);
        ChallengePresenterData challengePresentersData_3 = new ChallengePresenterData(3);

        challengePresenters.Add(challengePresentersData_1);
        challengePresenters.Add(challengePresentersData_2);
        challengePresenters.Add(challengePresentersData_3);

        return challengePresenters.ToArray();
    }
    #endregion Challenge Presenter Data

    #region HexagonData
    public HexagonData GetHexagonDataByID(int IDHex)
    {
        if (_cacheHexagonData.ContainsKey(IDHex))
        {
            return _cacheHexagonData[IDHex];
        }

        for (int i = 0; i < _hexagonDatas.Length; i++)
        {
            if (_hexagonDatas[i].ID == IDHex)
            {
                _cacheHexagonData[IDHex] = _hexagonDatas[i];
                return _cacheHexagonData[IDHex];
            }
        }

        return null;
    }

    public HexagonData[] GetAllHexagonData()
    {
        return _hexagonDatas;
    }

    private HexagonData[] LoadHexagonData()
    {
        //return CreateHexagonData();
        HexagonData[] datas = null; //FirebaseManager.instance.GetRemoteHexagons();

        if (datas == null)
        {
            datas = LoadLocalHexagonDatas();
        }

        return datas;
    }
    private HexagonData[] LoadLocalHexagonDatas()
    {
        string key = "Hexagons";
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format("Config/Data/{0}", key));

        if (textAsset != null)
        {
            return JsonConvert.DeserializeObject<HexagonData[]>(textAsset.text.Trim());
        }

        return null;
    }
    #endregion HexagonData

    #region Relic
    private Sprite GetRelicArt(string path)
    {
        if(_cacheRelicSprite.ContainsKey(path))
        {
            return (Sprite) _cacheRelicSprite[path];
        }

        Sprite art = Resources.Load<Sprite>(path);
        _cacheRelicSprite[path] = art;
        return art;
    }

    public Sprite GetRelicSpriteByID(int IDRelic)
    {
        string key = string.Format("Relic_{0}", IDRelic);
        string path = string.Format("Relics/{0}", key);

        return GetRelicArt(path);
    }

    public RelicData GetRelicDataByID(int IDRelic)
    {
        for (int i = 0; i < _relicDatas.Length; i++)
        {
            if (_relicDatas[i].ID == IDRelic)
            {
                return _relicDatas[i];
            }
        }

        return null;
    }

    #endregion Relic

    #region Gallery
    public GalleryData GetGalleryDataByID(int IDGallery)
    {
        for (int i = 0; i < _galleryDatas.Length; i++)
        {
            if (_galleryDatas[i].ID == IDGallery)
            {
                return _galleryDatas[i];
            }
        }

        return null;
    }

    private GalleryData[] CreateGalleryDatas()
    {
        List<GalleryData> galleryDatas = new List<GalleryData>();
        GalleryData gd_1 = new GalleryData();
        gd_1.ID = 1;
        gd_1.Name = "Gallery 01";
        gd_1.Capacity = 9;
        gd_1.IDRelics = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        GalleryData gd_2 = new GalleryData();
        gd_2.ID = 2;
        gd_2.Name = "Gallery 02";
        gd_2.Capacity = 9;
        gd_2.IDRelics = new int[] { 9, 10, 11, 12, 13, 14, 15, 16 };

        GalleryData gd_3 = new GalleryData();
        gd_3.ID = 3;
        gd_3.Name = "Gallery 03";
        gd_3.Capacity = 9;
        gd_3.IDRelics = new int[] { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27,28 };

        galleryDatas.Add(gd_1);
        galleryDatas.Add(gd_2);
        galleryDatas.Add(gd_3);
        return galleryDatas.ToArray();
    }

    private RelicData[] createRelicData()
    {
        List<RelicData> data = new List<RelicData>();
        RelicData relic_1 = new RelicData();
        relic_1.ID = 1;
        relic_1.Description = "This is Description of relic 1";
        relic_1.Name = "Relic 1";
        relic_1.Timer = 20;
        relic_1.Coin = 5;
        relic_1.ArtPath = "Relics/1";
        data.Add(relic_1);

        RelicData relic_2 = new RelicData();
        relic_2.ID = 2;
        relic_2.Description = "This is Description of relic 2";
        relic_2.Name = "Relic 2";
        relic_2.Timer = 22;
        relic_2.Coin = 6;
        relic_2.ArtPath = "Relics/2";
        data.Add(relic_2);

        RelicData relic_3 = new RelicData();
        relic_3.ID = 3;
        relic_3.Description = "This is Description of relic 3";
        relic_3.Name = "Relic 3";
        relic_3.Timer = 23;
        relic_3.Coin = 7;
        relic_3.ArtPath = "Relics/3";
        data.Add(relic_3);

        RelicData relic_4 = new RelicData();
        relic_4.ID = 4;
        relic_4.Description = "This is Description of relic 4";
        relic_4.Name = "Relic 4";
        relic_4.Timer = 24;
        relic_4.Coin = 14;
        relic_4.ArtPath = "Relics/4";
        data.Add(relic_4);

        RelicData relic_5 = new RelicData();
        relic_5.ID = 5;
        relic_5.Description = "This is Description of relic 5";
        relic_5.Name = "Relic 5";
        relic_5.Timer = 23;
        relic_5.Coin = 7;
        relic_5.ArtPath = "Relics/3";
        data.Add(relic_5);

        RelicData relic_6 = new RelicData();
        relic_6.ID = 6;
        relic_6.Description = "This is Description of relic 6";
        relic_6.Name = "Relic 6";
        relic_6.Timer = 23;
        relic_6.Coin = 7;
        relic_6.ArtPath = "Relics/6";
        data.Add(relic_6);

        RelicData relic_7 = new RelicData();
        relic_7.ID = 7;
        relic_7.Description = "This is Description of relic 7";
        relic_7.Name = "Relic 7";
        relic_7.Timer = 23;
        relic_7.Coin = 7;
        relic_7.ArtPath = "Relics/3";
        data.Add(relic_7);

        RelicData relic_8 = new RelicData();
        relic_8.ID = 8;
        relic_8.Description = "This is Description of relic 8";
        relic_8.Name = "Relic 8";
        relic_8.Timer = 23;
        relic_8.Coin = 7;
        relic_8.ArtPath = "Relics/3";
        data.Add(relic_8);

        RelicData relic_9 = new RelicData();
        relic_9.ID = 9;
        relic_9.Description = "This is Description of relic 9";
        relic_9.Name = "Relic 9";
        relic_9.Timer = 23;
        relic_9.Coin = 7;
        relic_9.ArtPath = "Relics/3";
        data.Add(relic_9);

        RelicData relic_10 = new RelicData();
        relic_10.ID = 10;
        relic_10.Description = "This is Description of relic 10";
        relic_10.Name = "Relic 10";
        relic_10.Timer = 23;
        relic_10.Coin = 7;
        relic_10.ArtPath = "Relics/3";
        data.Add(relic_10);

        RelicData relic_11 = new RelicData();
        relic_11.ID = 11;
        relic_11.Description = "This is Description of relic 11";
        relic_11.Name = "Relic 11";
        relic_11.Timer = 23;
        relic_11.Coin = 7;
        relic_11.ArtPath = "Relics/3";
        data.Add(relic_11);

        RelicData relic_12 = new RelicData();
        relic_12.ID = 12;
        relic_12.Description = "This is Description of relic 12";
        relic_12.Name = "Relic 12";
        relic_12.Timer = 23;
        relic_12.Coin = 7;
        relic_12.ArtPath = "Relics/3";
        data.Add(relic_12);

        RelicData relic_13 = new RelicData();
        relic_13.ID = 13;
        relic_13.Description = "This is Description of relic 13";
        relic_13.Name = "Relic 13";
        relic_13.Timer = 23;
        relic_13.Coin = 7;
        relic_13.ArtPath = "Relics/3";
        data.Add(relic_13);

        RelicData relic_14 = new RelicData();
        relic_14.ID = 14;
        relic_14.Description = "This is Description of relic 14";
        relic_14.Name = "Relic 14";
        relic_14.Timer = 23;
        relic_14.Coin = 7;
        relic_14.ArtPath = "Relics/3";
        data.Add(relic_14);

        RelicData relic_15 = new RelicData();
        relic_15.ID = 15;
        relic_15.Description = "This is Description of relic 15";
        relic_15.Name = "Relic 15";
        relic_15.Timer = 23;
        relic_15.Coin = 7;
        relic_15.ArtPath = "Relics/3";
        data.Add(relic_15);

        RelicData relic_16 = new RelicData();
        relic_16.ID = 16;
        relic_16.Description = "This is Description of relic 16";
        relic_16.Name = "Relic 16";
        relic_16.Timer = 23;
        relic_16.Coin = 7;
        relic_16.ArtPath = "Relics/3";
        data.Add(relic_16);

        RelicData relic_17 = new RelicData();
        relic_17.ID = 17;
        relic_17.Description = "This is Description of relic 17";
        relic_17.Name = "Relic 17";
        relic_17.Timer = 23;
        relic_17.Coin = 7;
        relic_17.ArtPath = "Relics/3";
        data.Add(relic_17);

        RelicData relic_18 = new RelicData();
        relic_18.ID = 18;
        relic_18.Description = "This is Description of relic 18";
        relic_18.Name = "Relic 18";
        relic_18.Timer = 23;
        relic_18.Coin = 7;
        relic_18.ArtPath = "Relics/3";
        data.Add(relic_18);

        RelicData relic_19 = new RelicData();
        relic_19.ID = 19;
        relic_19.Description = "This is Description of relic 19";
        relic_19.Name = "Relic 19";
        relic_19.Timer = 23;
        relic_19.Coin = 7;
        relic_19.ArtPath = "Relics/3";
        data.Add(relic_19);

        RelicData relic_20 = new RelicData();
        relic_20.ID = 20;
        relic_20.Description = "This is Description of relic 20";
        relic_20.Name = "Relic 20";
        relic_20.Timer = 23;
        relic_20.Coin = 7;
        relic_20.ArtPath = "Relics/3";
        data.Add(relic_20);

        RelicData relic_21 = new RelicData();
        relic_21.ID = 21;
        relic_21.Description = "This is Description of relic 21";
        relic_21.Name = "Relic 21";
        relic_21.Timer = 23;
        relic_21.Coin = 7;
        relic_21.ArtPath = "Relics/3";
        data.Add(relic_21);

        RelicData relic_22 = new RelicData();
        relic_22.ID = 22;
        relic_22.Description = "This is Description of relic 22";
        relic_22.Name = "Relic 22";
        relic_22.Timer = 23;
        relic_22.Coin = 7;
        relic_22.ArtPath = "Relics/3";
        data.Add(relic_22);

        RelicData relic_23 = new RelicData();
        relic_23.ID = 23;
        relic_23.Description = "This is Description of relic 23";
        relic_23.Name = "Relic 23";
        relic_23.Timer = 23;
        relic_23.Coin = 7;
        relic_23.ArtPath = "Relics/3";
        data.Add(relic_23);

        RelicData relic_24 = new RelicData();
        relic_24.ID = 24;
        relic_24.Description = "This is Description of relic 24";
        relic_24.Name = "Relic 24";
        relic_24.Timer = 22;
        relic_24.Coin = 7;
        relic_24.ArtPath = "Relics/3";
        data.Add(relic_24);

        RelicData relic_25 = new RelicData();
        relic_25.ID = 25;
        relic_25.Description = "This is Description of relic 25";
        relic_25.Name = "Relic 25";
        relic_25.Timer = 22;
        relic_25.Coin = 7;
        relic_25.ArtPath = "Relics/3";
        data.Add(relic_25);

        RelicData relic_26 = new RelicData();
        relic_26.ID = 26;
        relic_26.Description = "This is Description of relic 26";
        relic_26.Name = "Relic 26";
        relic_26.Timer = 22;
        relic_26.Coin = 7;
        relic_26.ArtPath = "Relics/3";
        data.Add(relic_26);

        RelicData relic_27 = new RelicData();
        relic_27.ID = 27;
        relic_27.Description = "This is Description of relic 27";
        relic_27.Name = "Relic 27";
        relic_27.Timer = 22;
        relic_27.Coin = 7;
        relic_27.ArtPath = "Relics/3";
        data.Add(relic_27);

        RelicData relic_28 = new RelicData();
        relic_28.ID = 28;
        relic_28.Description = "This is Description of relic 28";
        relic_28.Name = "Relic 28";
        relic_28.Timer = 22;
        relic_28.Coin = 7;
        relic_28.ArtPath = "Relics/3";
        data.Add(relic_28);


        return data.ToArray();
    }
    #endregion Gallery
}
