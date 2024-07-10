using Mul21_Lib;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class ResourceManager : PersistentMonoSingleton<ResourceManager>
{
    private const int TEST_IDLEVEL = 1;

    private Dictionary<int, LevelData> _levelDataDict = new Dictionary<int, LevelData>();

    private LevelPresenterData[] _levelPresenterDatas;
    private Dictionary<int, LevelPresenterData> _levelPresenterDatasDict = new Dictionary<int, LevelPresenterData>();

    private ChallengeData _challengeData;

    private HexagonData[] _hexagonDatas;
    private Dictionary<int, HexagonData> _cacheHexagonData = new Dictionary<int, HexagonData>();

    public void LoadResource()
    {
        _challengeData = LoadChallengeData();
        _levelPresenterDatas = LoadLevelPresenterDatas();
        _hexagonDatas = LoadHexagonData();
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
        int IDLevel = Random.Range(0, IDMax);

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
    public ChallengeData GetChallengeByID(int IDChallenge = TEST_IDLEVEL)
    {
        if (_challengeData != null)
        {
            _challengeData = LoadLocalChallengeData($"Challenge_{IDChallenge}");
        }

        return _challengeData;
    }

    private ChallengeData LoadChallengeData()
    {
        return LoadLocalChallengeData("Challenge_1");
    }

    private ChallengeData LoadLocalChallengeData(string key)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format("Config/Data/Challenges/{0}", key));
        if (textAsset != null)
        {
            Debug.Log("Key: " + textAsset.text.Trim());
            return JsonConvert.DeserializeObject<ChallengeData>(textAsset.text.Trim());
        }
        return null;
    }

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

    private HexagonData[] CreateHexagonData()
    {
        HexagonData hexa1 = new HexagonData(1, "#333333");
        HexagonData hexa2 = new HexagonData(2, "#FF55DF");
        HexagonData hexa3 = new HexagonData(3, "#FFC700");
        HexagonData hexa4 = new HexagonData(4, "#219C90");

        return new HexagonData[] { hexa1, hexa2, hexa3, hexa4 };
    }

    public HexagonData[] GetAllHexagonData()
    {
        return _hexagonDatas;
    }

    private HexagonData[] LoadHexagonData()
    {
        //return CreateHexagonData();
        return LoadLocalHexagonDatas();
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
