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
            _levelData = LoadLocalLevelData($"Level_{IDLevel}");
        }

        return _levelData;
    }

    private LevelData LoadLevelData()
    {
        return LoadLocalLevelData("Level_1");
    }

    private LevelData LoadLocalLevelData(string key)
    {
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
        List<LevelPresenterData> levelPresenterDatas = new List<LevelPresenterData>();
        LevelPresenterData levelPresenter1 = new LevelPresenterData(1, 100);
        LevelPresenterData levelPresenter2 = new LevelPresenterData(2, 200);
        levelPresenterDatas.Add(levelPresenter1);
        levelPresenterDatas.Add(levelPresenter2);
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


    //TEST ONLY
    //private ChallengeData CreateChallengeData()
    //{
    //    StackHexagonData stack1 = new StackHexagonData(new string[] { "#FFC700", "#FFC700", "#219C90", "#219C90", "#219C90", "#219C90", "#219C90", "#219C90" });
    //    StackHexagonData stack2 = new StackHexagonData(new string[] { "#FF55DF", "#FF55DF", "#FF55DF", "#FF55DF", "#FF55DF", "#FF55DF", "#FFC700"});
    //    StackHexagonData stack3 = new StackHexagonData(new string[] { "#219C90", "#219C90", "#219C90", "#219C90", "#FFC700", "#FFC700", "#FFC700", "#FFC700", "#FFC700"});
    //    StackHexagonData stack4 = new StackHexagonData(new string[] { "#FFC700", "#FFC700", "#FFC700", "#FFC700", "#FFC700", "#FFC700", "#219C90"});
    //    StackHexagonData stack5 = new StackHexagonData(new string[] { "#219C90", "#219C90", "#219C90", "#219C90", "#219C90", "#219C90", "#219C90"});
    //    StackHexagonData stack6 = new StackHexagonData(new string[] { "#FF55DF", "#FF55DF", "#FF55DF", "#FF55DF", "#FF55DF", "#FF55DF", "#FF55DF", "#219C90", "#219C90"});

    //    StackQueueData stackQueueData = new StackQueueData(new StackHexagonData[] {stack1, stack2, stack3, stack4, stack5, stack6});
    //    ChallengeData challengeData = new ChallengeData(stackQueueData);
    //    challengeData.DebugLogObject();

    //    return challengeData;
    //}
    // END

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
