using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mul21_Lib;
using System;
using System.Threading.Tasks;

public class FirebaseManager : PersistentMonoSingleton<FirebaseManager>
{
    #region Template
    private FB_RemoteConfig _remoteConfig;
    Firebase.FirebaseApp appOnCloud = null;

    private async void Start()
    {
        await ConnectToFirebase();
    }

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
}
