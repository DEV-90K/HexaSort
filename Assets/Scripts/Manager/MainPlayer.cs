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
        _PlayerData = LoadPlayerData();
        Debug.Log("PlayerData Load");
        _PlayerData.DebugLogObject();

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

        return Array.Empty<GalleryRelicData>();
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

    public int GetMaterial() => _PlayerData.Material;
    public void AddMaterial(int amount) => _PlayerData.Material += amount;
    public void SubMaterial(int amount) => _PlayerData.Material -= amount;

    private void CachePlayerLevelData()
    {
        LevelData levelData = LevelManager.instance.GetLevelData();
        LevelPresenterData levelPresenterData = LevelManager.instance.GetPresenterData();

        _PlayerData.PlayerLevel.UpdateLevelData(levelData);
        _PlayerData.PlayerLevel.UpdateLevelPresenterData(levelPresenterData);
    } 

    public void CacheIDLevel(int IDLevel)
    {
        if (IDLevel > _PlayerData.PlayerLevel.IDLevel)
        {
            _PlayerData.PlayerLevel.UpdateIDLevel(IDLevel);
        }
    }
    
    public PlayerLevelData GetPlayerLevelData()
    {
        return _PlayerData.PlayerLevel;
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
            //TEST
            playerData = null;
        }

        if(playerData == null)
        {
            Debug.Log("Load From Local");
            playerData = LoadLocalPlayerData();

            //TEST
            playerData = null;
        }

        if(playerData == null)
        {
            Debug.Log("Create New");
            playerData = CreatePlayerData();
        }

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
        LevelPresenterData levelPresenterData = ResourceManager.instance.GetLevelPresenterDataByID(1);
        int IDLevel = 2;

        PlayerLevelData levelData = new PlayerLevelData(IDLevel, level, levelPresenterData);

        int coin = 50;
        int material = 20;

        return new PlayerData(coin, material, levelData);
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

    private void OnApplicationQuit()
    {
        Debug.Log("On Application Quit");

        if(GameManager.instance.IsState(GameState.LEVEL_PLAYING))
        {
            LevelManager.instance.CacheCurrentLevelPlayingData();
        }

        CachePlayerLevelData();
        SavePlayerDataFromPlayerPrefab(_PlayerData);
        Debug.Log("PlayerData Save");
        _PlayerData.DebugLogObject();
    }

    //[RuntimeInitializeOnLoadMethod]
    //private static void OnApplicationLoad()
    //{
    //    Debug.Log("On Application Load");
    //    Instance.DeletePlayerDataFromPlayerPrefab();
    //}
}
