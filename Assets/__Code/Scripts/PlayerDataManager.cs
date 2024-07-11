using Mul21_Lib;
using Newtonsoft.Json;
using System;
using UnityEngine;

public class PlayerDataManager : PersistentMonoSingleton<PlayerDataManager> 
{
    private const string KEY_PLAYER = "PLAYER_DATA";
    private PlayerData _PlayerData;

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

    private PlayerData LoadPlayerData()
    {
        return LoadPlayDataFromPlayerPrefab();
    }

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

    public void LoadData()
    {
        //throw new NotImplementedException();
    }

    #endregion PlayerPrefab Load
}
