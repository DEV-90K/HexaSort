using Mul21_Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    LOADING,
    PLAYING,
    PAUSE,
    FINISH
}

public class GameManager : PersistentMonoSingleton<PlayerDataManager>
{
    public static Action<GameState> OnChangeState;
    private static GameState state;

    public static void ChangeState(GameState newState)
    {
        GameManager.state = newState;
        OnChangeState?.Invoke(GameManager.state);
    }

    public static bool IsState(GameState state) => GameManager.state == state;

    protected override void Awake()
    {
        base.Awake();
        SetUp();
    }

    private void Start()
    {
        ChangeState(GameState.LOADING);
    }

    private void SetUp()
    {
        //tranh viec nguoi choi cham da diem vao man hinh
        Input.multiTouchEnabled = false;
        //target frame rate ve 60 fps
        Application.targetFrameRate = 60;
        //tranh viec tat man hinh
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //xu tai tho
        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
    }
}
