using Mul21_Lib;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : PersistentMonoSingleton<ResourceManager>
{
    private LevelData _levelData;

    protected override void Awake()
    {
        base.Awake();

        _levelData = LoadLevelData();
    }

    public LevelData GetLevelByID()
    {
        if(_levelData != null)
        {
            _levelData = LoadLocalLevelData("Level_1");
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
        Debug.Log("textAsset != null: " + textAsset != null);
        Debug.Log("Data: " + textAsset.text.Trim());

        if (textAsset != null && !string.IsNullOrEmpty(textAsset.text.Trim()))
        {
            Debug.Log("Key: " + textAsset.text.Trim());
            return JsonConvert.DeserializeObject<LevelData>(textAsset.text.Trim());
        }
        Debug.Log("Not Found Level At Local: " + key);
        return null;
    }   
}
