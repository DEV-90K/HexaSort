using Mul21_Lib;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : PersistentMonoSingleton<ResourceManager>
{
    private const int TEST_IDLEVEL = 1;

    private LevelData _levelData;
    private LevelPresenterData[] _levelPresenterDatas;

    protected override void Awake()
    {
        base.Awake();

        _levelData = LoadLevelData();
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
            Debug.Log("Key: " + textAsset.text.Trim());
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
            Debug.Log("Key: " + textAsset.text.Trim());
            return JsonConvert.DeserializeObject<LevelPresenterData[]>(textAsset.text.Trim());
        }
        Debug.Log("Not Found PresenterDatas At Local: " + key);
        return null;
    }
}
