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

    public void LoadResource()
    {
        _levelPresenterDatas = LoadLevelPresenterDatas();
        _challengePresenterDatas = LoadChallengePresenterDatas();

        _hexagonDatas = LoadHexagonData();
        _mechanicConfig = LoadMechanicConfig();

        //TEST
        _relicDatas = createRelicData();
    }

    #region Mechanic Config

    public StackConfig GetStackConfig()
    {
        return _mechanicConfig.StackConfig;
    }

    private MechanicConfig LoadMechanicConfig()
    {
        MechanicConfig config = FirebaseManager.instance.LoadRemoteMechanicConfig();

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
        LevelData levelData = FirebaseManager.instance.GetRemoteLevelData(IDLevel);

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
        LevelPresenterData[] presenterDatas = FirebaseManager.instance.GetRemoteLevelPresenterDatas();

        if(presenterDatas == null)
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
        ChallengeData challengeData = FirebaseManager.instance.GetRemoteChallengeData(IDChallenge);

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
        ChallengePresenterData[] presenterDatas = FirebaseManager.instance.GetRemoteChallengePresenterDatas();

        if (presenterDatas == null)
        {
            presenterDatas = LoadLocalChallengePresenterDatas();
        }

        if(presenterDatas == null)
        {
            presenterDatas = CreateChallengePresenterDatas();
        }

        presenterDatas.DebugLogObject();
        
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
        ChallengePresenterData challengePresentersData_1 = new ChallengePresenterData(1, 20, 15);
        ChallengePresenterData challengePresentersData_2 = new ChallengePresenterData(2, 25, 35);
        ChallengePresenterData challengePresentersData_3 = new ChallengePresenterData(3, 40, 50);

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
        HexagonData[] datas = FirebaseManager.instance.GetRemoteHexagons();

        if(datas == null)
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
    public GalleryData GetGalleryData()
    {
        return createGalleryData();
    }
    private GalleryData createGalleryData()
    {
        GalleryData galleryData = new GalleryData();
        galleryData.ID = 1;
        galleryData.Name = " Gallery 01";
        galleryData.Capacity = 9;
        galleryData.IDRelics = new int[] { 1, 2, 3 };

        return galleryData;
    }

    private GalleryRelicData createGalleryRelicData()
    {
        GalleryRelicData galleryRelicData = new GalleryRelicData();
        galleryRelicData.IDGallery = 1;
        galleryRelicData.IDRelic = 2;
        galleryRelicData.Position = 4;

        return galleryRelicData;
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

        return data.ToArray();
    }
    #endregion Gallery
}
