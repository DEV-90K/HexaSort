using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FB_RemoteConfig
{
    Firebase.FirebaseApp appOnCloud = null;
    Firebase.RemoteConfig.FirebaseRemoteConfig remoteConfig = null;

    public FB_RemoteConfig(Firebase.FirebaseApp appOnCloud)
    {
        //ConnectToFirebaseApp();
        this.appOnCloud = appOnCloud;
    }

    private void ConnectToFirebaseApp()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWith(task =>
            {
                if (task.Result == Firebase.DependencyStatus.Available)
                {
                    appOnCloud = Firebase.FirebaseApp.DefaultInstance;

                    ConnectToFirebaseRemoteConfig();
                }
                else
                {
                    Debug.Log(String.Format("[Firebase] Could not connect to Firebase: {0}", task.Result));
                }

            });
    }

    public Task ConnectToFirebaseRemoteConfig()
    {
        Debug.Log("Fetching remote config....");
        Task fetchData = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
            .FetchAsync(TimeSpan.Zero);
        return fetchData.ContinueWithOnMainThread(FetchRemoteConfigComplete);
    }

    private void FetchRemoteConfigComplete(Task fetchData)
    {
        if (fetchData.IsCanceled)
        {
            Debug.Log("[Firebase] Fetch canceled.");
        }
        else if (fetchData.IsFaulted)
        {
            Debug.Log("[Firebase] Fetch encountered an error.");
        }
        else if (fetchData.IsCompleted)
        {
            Debug.Log("[Firebase] Fetch completed successfully!");
        }

        remoteConfig = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance;
        Firebase.RemoteConfig.ConfigInfo configInfo = remoteConfig.Info;

        switch (configInfo.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                Debug.Log(String.Format("[Firebase] Remote data loaded and ready (last fetch time {0}).", configInfo.FetchTime));
                OnRemoteConfigLoadScuccess();
                break;
            case LastFetchStatus.Failure:
                Debug.Log(String.Format("[Firebase] Remove Config was configInfo error {0} status {1}", nameof(configInfo.LastFetchStatus), configInfo.LastFetchStatus));
                OnRemoteConfigLoadError();
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Debug.Log("[Firebase] Latest Fetch call still pending.");
                break;
        }


        //if (configInfo.LastFetchStatus != Firebase.FB_RemoteConfig.LastFetchStatus.Success)
        //{
        //    Debug.Log(String.Format("[Firebase] Remove Config was configInfo error {0} status {1}", nameof(configInfo.LastFetchStatus), configInfo.LastFetchStatus));
        //    OnRemoteConfigLoadError();
        //    return;
        //}

        //OnRemoteConfigLoadScuccess();
    }

    private void OnRemoteConfigLoadScuccess()
    {
        Debug.Log("[Firebase] On Remote Config Load Scuccess");
        //Fetch successful. Parameter values must be activated to use
        remoteConfig.ActivateAsync()
            .ContinueWithOnMainThread(
            task =>
            {
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {remoteConfig.Info.FetchTime}");
                Debug.Log($"Total values: " + remoteConfig.AllValues.Count);
                foreach (var item in remoteConfig.AllValues)
                {
                    Debug.Log("Key: " + item.Key);
                    Debug.Log("Value: " + item.Value.StringValue);
                }
            });
    }

    private void OnRemoteConfigLoadError()
    {
        Debug.Log("[Firebase] On Remote Config Load Error");
        remoteConfig.SetDefaultsAsync(InitDefaultValues())
            .ContinueWithOnMainThread(
            task =>
            {
                foreach (var item in remoteConfig.AllValues)
                {
                    Debug.Log("Key: " + item.Key);
                    Debug.Log("Value: " + item.Value.StringValue);
                }

            });
    }

    private Dictionary<string, object> InitDefaultValues()
    {
        Dictionary<string, object> defaultValues = new Dictionary<string, object>();
        return defaultValues;
    }

    public string GetRemoteConfigValue(string key)
    {
        try
        {
            return remoteConfig.GetValue(key).StringValue;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            return null;
        }
    }
}
