using Mul21_Lib;
using System;
using UnityEngine;

public enum GameState
{
    NONE,
    LOADING,
    PLAYING,
    PAUSE,
    FINISH
}

public class GameManager : PersistentMonoSingleton<GameManager>
{
    public static Action<GameState> OnChangeState;
    private GameState state;

    public void ChangeState(GameState newState)
    {
        state = newState;
        Debug.Log("GameState Change: " + EnumUtils.ParseString(state));
        OnChangeState?.Invoke(state);
    }

    public bool IsState(GameState state) => this.state == state;

    protected override void Awake()
    {
        base.Awake();
        SetUp();
        ChangeState(GameState.NONE);
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
        //int maxScreenHeight = 1280;
        //float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        //if (Screen.currentResolution.height > maxScreenHeight)
        //{
        //    Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        //}
    }
}
