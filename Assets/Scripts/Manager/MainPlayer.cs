using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : PersistentMonoSingleton<MainPlayer> 
{
    private const string KEY_PLAYER = "PLAYER_DATA";
    private PlayerData _PlayerData;

    private Dictionary<int, List<GalleryRelicData>> _DictGalleryRelic = new Dictionary<int, List<GalleryRelicData>>(); //int is ID of gallery    

    public PlayerData GetPlayerData()
    {
        if (_PlayerData != null)
        {
            return _PlayerData;
        }

        _PlayerData = LoadPlayerData();
        return _PlayerData;
    }

    public void SavePlayerData(PlayerData playerData)
    {
        SavePlayerDataFromPlayerPrefab(playerData);
    }

    public void LoadData()
    {
        LoadPlayerData();  
        
        List<GalleryRelicData> listData = new List<GalleryRelicData>();
        listData.Add(new GalleryRelicData(1, 2, 4, DateTime.Now.ToString(), GalleryRelicState.COLLECT));
        listData.Add(new GalleryRelicData(1, 1, 6, DateTime.Now.AddMinutes(-20).ToString(), GalleryRelicState.COLLECT));

        _DictGalleryRelic[1] = listData;
    }

    public GalleryRelicData[] GetGalleryRelicByID(int IDGallery)
    {
        if(_DictGalleryRelic.ContainsKey(IDGallery))
        {
            return _DictGalleryRelic[IDGallery].ToArray();
        }

        return null;
    }

    public void CollectGalleryRelic(GalleryRelicData galleryRelicData)
    {
        galleryRelicData.LastTimer = DateTime.Now.ToString();
        if(_DictGalleryRelic.ContainsKey(galleryRelicData.IDGallery))
        {
            _DictGalleryRelic[galleryRelicData.IDGallery].Add(galleryRelicData);
        }
        else
        {
            _DictGalleryRelic[galleryRelicData.IDGallery] = new List<GalleryRelicData> { galleryRelicData };
        }    
    }

    public int GetCoin() => _PlayerData.Coin;
    public void AddCoin(int amount) => _PlayerData.Coin += amount;
    public void SubCoin(int amount) => _PlayerData.Coin -= amount;
    public void CachePlayerLevelData()
    {
        LevelData levelData = LevelManager.instance.GetCurrentLevelPlayingData();
        int IDLevel = LevelManager.instance.GetCurrentLevelPlayingID();

        if(IDLevel > _PlayerData.PlayerLevel.IDLevel)
        {
            _PlayerData.PlayerLevel.UpdateIDLevel(IDLevel);
        }

        _PlayerData.PlayerLevel.UpdateLevelData(levelData);

        _PlayerData.DebugLogObject();
    } 
    
    #region Player Data
    private PlayerData LoadPlayerData()
    {
        Debug.Log("Load From PlayerPrefab");
        PlayerData playerData = LoadPlayDataFromPlayerPrefab();

        if(playerData == null)
        {
            Debug.Log("Load From Remote");
            playerData = FirebaseManager.instance.LoadRemotePlayerData(); //Fisrt Time Install Game
        }

        if(playerData == null)
        {
            Debug.Log("Load From Local");
            playerData = LoadLocalPlayerData();
        }

        if(playerData == null)
        {
            Debug.Log("Create New");
            playerData = CreatePlayerData();
        }

        playerData.DebugLogObject();

        return playerData;
    }

    private PlayerData LoadLocalPlayerData()
    {
        string key = "Player";

        TextAsset textAsset = Resources.Load<TextAsset>(string.Format("Config/Data/{0}", key));
        if (textAsset != null)
        {
            return JsonConvert.DeserializeObject<PlayerData>(textAsset.text.Trim());
        }
        return null;
    }

    private PlayerData CreatePlayerData()
    {
        LevelData level = ResourceManager.instance.GetLevelByID(1);
        int IDLevel = 2;

        PlayerLevelData levelData = new PlayerLevelData(IDLevel, level);

        int coin = 50;

        return new PlayerData(coin, levelData);
    }

    #endregion Player Data
    #region PlayerPrefab Load
    private PlayerData LoadPlayDataFromPlayerPrefab()
    {
        string txtData = PlayerPrefs.GetString(KEY_PLAYER);

        if (string.IsNullOrEmpty(txtData))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<PlayerData>(txtData.Trim());
    }

    private void SavePlayerDataFromPlayerPrefab(PlayerData data)
    {
        PlayerPrefs.SetString(KEY_PLAYER, JsonConvert.SerializeObject(data));
    }

    private void DeletePlayerDataFromPlayerPrefab()
    {
        PlayerPrefs.DeleteKey(KEY_PLAYER);
    }
    #endregion PlayerPrefab Load

    //private void OnApplicationQuit()
    //{
    //    Debug.Log("On Application Quit");
    //    CachePlayerLevelData();
    //    SavePlayerDataFromPlayerPrefab(_PlayerData);
    //}

    //[RuntimeInitializeOnLoadMethod]
    //private static void OnApplicationLoad()
    //{
    //    Debug.Log("On Application Load");
    //    Instance.DeletePlayerDataFromPlayerPrefab();
    //}
}
