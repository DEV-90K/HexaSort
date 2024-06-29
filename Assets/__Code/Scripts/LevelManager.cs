using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mul21_Lib;
using UnityUtils;
using System;

public class LevelManager : MonoSingleton<LevelManager>
{
    private enum LevelState
    {
        NONE,
        LOADING,
        PLAYING,
        PAUSED,
        FINISHED
    }

    [SerializeField]
    private GridManager _gridManager;
    [SerializeField]
    private StackManager _stackManager;

    private LevelData _levelData;
    private LevelPresenterData _presenterData;
    private ScreenLevel _sceenLevel;

    private int amountHexagon = 0;
    private LevelState levelState = LevelState.NONE;

    private void Start()
    {
        levelState = LevelState.NONE;

        Hexagon.OnVanish += Hexagon_OnVanish;
        StackMerger.OnStackMergeCompleted += StackMerge_OnStackMergeCompleted;

        InitTestLevel();
    }

    private void OnDestroy()
    {
        Hexagon.OnVanish -= Hexagon_OnVanish;
        StackMerger.OnStackMergeCompleted -= StackMerge_OnStackMergeCompleted;
    }

    private void InitTestLevel()
    {
        LevelData levelData = ResourceManager.Instance.GetLevelByID(1);
        LevelPresenterData presenterData = ResourceManager.instance.GetLevelPresenterDataByID(1);

        OnInit(levelData, presenterData);
    }

    public void OnInit(LevelData levelData, LevelPresenterData presenterData)
    {
        levelState = LevelState.LOADING;

        _levelData = levelData;
        _presenterData = presenterData;
        amountHexagon = 0;

        _gridManager.OnInit(levelData.Grid);
        _stackManager.OnInit();

        _sceenLevel = GUIManager.Instance.ShowScreen<ScreenLevel>(presenterData);

        levelState = LevelState.PLAYING;
    }

    public void OnReplay()
    {
        _stackManager.CollectRandomImmediate();
        _gridManager.CollectGridImmediate();

        OnInit(_levelData, _presenterData);
    }

    public void OnFinish()
    {
        Debug.Log("OnFinish");
        levelState = LevelState.FINISHED;

        _gridManager.CollectOccupied();
        _stackManager.CollectRandomed();
        _sceenLevel.Hide();

        //Invoke(nameof(InitTestLevel), 1f);
    }

    //Test Case Wrong: In Process Merge and Remove
    private void Hexagon_OnVanish()
    {
        if(levelState != LevelState.PLAYING)
        {
            return;
        }

        if(amountHexagon >= _presenterData.Goal)
        {
            amountHexagon = _presenterData.Goal;
        }
        else 
            amountHexagon += 1;

        _sceenLevel.OnChangeHexagon(amountHexagon);
    }

    private void StackMerge_OnStackMergeCompleted()
    {
        Debug.Log("Level State: " + EnumUtils.ParseString(levelState));
        if (levelState != LevelState.PLAYING)
        {
            return;
        }

        if (amountHexagon >= _presenterData.Goal)
        {
            OnFinish();
        }
    } 
}
