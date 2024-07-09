using Mul21_Lib;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class ResourceManager : PersistentMonoSingleton<ResourceManager>
{
    private const int TEST_IDLEVEL = 1;

    private LevelData _levelData;
    private LevelPresenterData[] _levelPresenterDatas;

    private ChallengeData _challengeData;

    private HexagonData[] _hexagonDatas;
    private Dictionary<int, HexagonData> _cacheHexagonData = new Dictionary<int, HexagonData>();

    public void LoadResource()
    {
        _levelData = LoadLevelData();
        _challengeData = LoadChallengeData();
        _challengeData.DebugLogObject();
        _hexagonDatas = LoadHexagonData();
        _hexagonDatas.DebugLogObject();
    }

    public LevelData GetLevelByID(int IDLevel = TEST_IDLEVEL)
    {
        if(_levelData != null)
        {
            _levelData = LoadLevelData(IDLevel);
        }

        return _levelData;
    }

    private LevelData LoadLevelData(int IDLevel = TEST_IDLEVEL)
    {
        Debug.Log("Load LevelData Remote");
        LevelData levelData = FirebaseManager.instance.GetRemoteLevelData(IDLevel);

        if(levelData == null)
        {
            Debug.Log("LoadLocalLevelData");
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

    public LevelPresenterData GetLevelPresenterDataByID(int IDLevel = TEST_IDLEVEL)
    {
        if(_levelPresenterDatas == null)
        {
            _levelPresenterDatas = LoadLevelPresenterDatas();
        }

        if(_levelPresenterDatas == null)
        {
            _levelPresenterDatas = CreateLevelPresenterDatas();
        }

        for (int i = 0; i < _levelPresenterDatas.Length; i++)
        {
            if (_levelPresenterDatas[i].Level == IDLevel)
            {
                return _levelPresenterDatas[i];
            }
        }

        return null;
    }

    //TEST ONLY
    private LevelPresenterData[] CreateLevelPresenterDatas()
    {
        Debug.Log("CreateLevelPresenterDatas");
        List<LevelPresenterData> levelPresenterDatas = new List<LevelPresenterData>();
        LevelPresenterData levelPresenter1 = new LevelPresenterData(1, 100);
        LevelPresenterData levelPresenter2 = new LevelPresenterData(2, 200);
        levelPresenterDatas.Add(levelPresenter1);
        levelPresenterDatas.Add(levelPresenter2);
        levelPresenterDatas.ToArray().DebugLogObject();
        return levelPresenterDatas.ToArray();
    }
    //END

    private LevelPresenterData[] LoadLevelPresenterDatas()
    {
        return LoadLocalPresenterDatas();
    }

    private LevelPresenterData[] LoadLocalPresenterDatas()
    {
        string key = "LevelPresenters";
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format("Config/Presenter/{0}", key));

        if (textAsset != null)
        {
            return JsonConvert.DeserializeObject<LevelPresenterData[]>(textAsset.text.Trim());
        }

        return null;
    }

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
