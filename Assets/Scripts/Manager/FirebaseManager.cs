using UnityEngine;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class FirebaseManager : PersistentMonoSingleton<FirebaseManager>
{
    #region Template
    private FB_RemoteConfig _remoteConfig;
    Firebase.FirebaseApp appOnCloud = null;

    public async Task ConnectToFirebase()
    {
        await ConnectToFirebaseApp();

        //Connecto to all Firebase Service
        if (appOnCloud != null)
        {
            _remoteConfig = new FB_RemoteConfig(appOnCloud);
            await _remoteConfig.ConnectToFirebaseRemoteConfig();
        }
    }

    private async Task ConnectToFirebaseApp()
    {
        await Firebase.FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWith(task =>
            {
                if (task.Result == Firebase.DependencyStatus.Available)
                {
                    appOnCloud = Firebase.FirebaseApp.DefaultInstance;
                }
                else
                {
                    Debug.Log(String.Format("[Firebase] Could not connect to Firebase: {0}", task.Result));
                }
            });
    }
    #endregion Template

    public LevelData GetRemoteLevelData(int iDLevel)
    {
        string key = "Level_" + iDLevel;

        try
        {
            string value = _remoteConfig.GetRemoteConfigValue(key);
            LevelData data = JsonConvert.DeserializeObject<LevelData>(value);
            return data;
        }
        catch
        {
            return null;
        }
    }

    public LevelPresenterData[] GetRemoteLevelPresenterDatas()
    {
        string key = "LevelPresenters";
        try
        {
            string value = _remoteConfig.GetRemoteConfigValue(key);
            LevelPresenterData[] data = JsonConvert.DeserializeObject<LevelPresenterData[]>(value);
            return data;
        }
        catch
        {
            return null;
        }
    }

    public HexagonData[] GetRemoteHexagons()
    {
        string key = "Hexagons";

        try
        {
            string value = _remoteConfig.GetRemoteConfigValue(key);
            HexagonData[] data = JsonConvert.DeserializeObject<HexagonData[]>(value);
            return data;
        }
        catch
        {
            return null;
        }
    }

    public ChallengeData GetRemoteChallengeData(int iDChallenge)
    {
        string key = "Challenge_" + iDChallenge;

        try
        {
            string value = _remoteConfig.GetRemoteConfigValue(key);
            ChallengeData data = JsonConvert.DeserializeObject<ChallengeData>(value);
            return data;
        }
        catch
        {
            return null;
        }
    }

    internal MechanicConfig LoadRemoteMechanicConfig()
    {
        string key = "Mechanic";

        try
        {
            string value = _remoteConfig.GetRemoteConfigValue(key);
            MechanicConfig config = JsonConvert.DeserializeObject<MechanicConfig>(value);
            return config;
        }
        catch
        {
            return null;
        }
    }

    internal PlayerData LoadRemotePlayerData()
    {
        string key = "Player";

        try
        {
            string value = _remoteConfig.GetRemoteConfigValue(key);
            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(value);
            return data;
        }
        catch
        {
            return null;
        }
    }
}
