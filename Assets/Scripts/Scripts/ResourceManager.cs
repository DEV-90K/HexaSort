using Mul21_Lib;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class ResourceManager : PersistentMonoSingleton<ResourceManager>
{
    private const int TEST_IDLEVEL = 1;

    private MechanicConfig _mechanicConfig;
    
    private Dictionary<int, LevelData> _levelDataDict = new Dictionary<int, LevelData>();

    private LevelPresenterData[] _levelPresenterDatas;
    private Dictionary<int, LevelPresenterData> _levelPresenterDatasDict = new Dictionary<int, LevelPresenterData>();

    private Dictionary<int, ChallengeData> _challengeDataDict = new Dictionary<int, ChallengeData>();

    private HexagonData[] _hexagonDatas;
    private Dictionary<int, HexagonData> _cacheHexagonData = new Dictionary<int, HexagonData>();

    public void LoadResource()
    {
        _levelPresenterDatas = LoadLevelPresenterDatas();
        _hexagonDatas = LoadHexagonData();
        _mechanicConfig = LoadMechanicConfig();
    }

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
        int IDLevel = Random.Range(1, IDMax);

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
        int IDXLevel = Random.Range(0, _levelPresenterDatas.Length);
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
        int IDChallenge = Random.Range(1, IDMax);

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
}
