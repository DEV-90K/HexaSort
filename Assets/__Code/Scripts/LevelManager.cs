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

    [SerializeField]
    private Hammer _hammer;

    private LevelData _levelData;
    private LevelPresenterData _presenterData;

    private int amountHexagon = 0;
    private LevelState levelState = LevelState.NONE;

    private void OnEnable()
    {
        StackMerger.OnStackMergeCompleted += StackMerge_OnStackMergeCompleted;
    }

    private void OnDisable()
    {
        StackMerger.OnStackMergeCompleted -= StackMerge_OnStackMergeCompleted;
    }

    private void Start()
    {
        levelState = LevelState.NONE;     
        _hammer.gameObject.SetActive(false);
        InitTestLevel();
    }

    public LevelPresenterData GetPresenterData()
    {
        return _presenterData;
    }

    public int GetAmountHexagon()
    {
        return amountHexagon;
    }

    public void UpdateAmountHexagon(int amount)
    {
        amountHexagon = amount;
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

        GUIManager.Instance.ShowScreen<ScreenLevel>(presenterData);
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
        if (levelState == LevelState.FINISHED)
            return;

        levelState = LevelState.FINISHED;

        _gridManager.CollectOccupied();
        _stackManager.CollectRandomed();

        Invoke(nameof(InitTestLevel), 1f);
    }

    #region Boost Hammer
    public void EnterBoostHammer()
    {
        GridHexagon[] gridHexagons = _gridManager.GetGridHexagonContainStack();
        foreach (GridHexagon grid in gridHexagons)
        {
            if(grid.CheckOccupied())
            {
                foreach(Hexagon hex in grid.StackOfCell.Hexagons)
                {
                    hex.EnableCollider();
                }
            }
        }

        _stackManager.DisableByBooster();
    }

    public void OnBoostHammer(Hexagon hexagon)
    {
        StartCoroutine(IE_OnBoostHammer(hexagon));
    }
    private IEnumerator IE_OnBoostHammer(Hexagon hexagon)
    {
        StackHexagon stackHexagon = hexagon.HexagonStack;
        Vector3 hammerPos = stackHexagon.GetTopPositon();
        _hammer.gameObject.SetActive(true);
        _hammer.transform.position = hammerPos;
        yield return _hammer.IE_HummerAction();
        yield return stackHexagon.IE_CollectPlayerHexagon();
        _hammer.gameObject.SetActive(false);
    }

    public void ExitBoostHammer()
    {        
        GridHexagon[] gridHexagons = _gridManager.GetGridHexagonContainStack();
        foreach (GridHexagon grid in gridHexagons)
        {
            if (grid.CheckOccupied())
            {
                foreach (Hexagon hex in grid.StackOfCell.Hexagons)
                {
                    hex.DisableCollider();
                }
            }
        }

        _stackManager.EnableByBooster();
    }
    #endregion Boost Hammer

    #region Boost Swap
    public void EnterBoostSwap()
    {
        Debug.Log("EnterBoostSwap");

        GridHexagon[] gridHexagons = _gridManager.GetGridHexagonContainStack();
        foreach (GridHexagon grid in gridHexagons)
        {
            if (grid.CheckOccupied())
            {
                foreach (Hexagon hex in grid.StackOfCell.Hexagons)
                {
                    hex.EnableCollider();
                }
            }
        }

        _stackManager.DisableByBooster();
    }

    public void OnBoostSwap(GridHexagon grid)
    {
        //_stackManager.Mer
        _stackManager.MergeStackIntoGrid(grid);
    }

    public void ExitBoostSwap()
    {
        Debug.Log("ExitBoostSwap");
        GridHexagon[] gridHexagons = _gridManager.GetGridHexagonContainStack();
        foreach (GridHexagon grid in gridHexagons)
        {
            if (grid.CheckOccupied())
            {
                foreach (Hexagon hex in grid.StackOfCell.Hexagons)
                {
                    hex.DisableCollider();
                }
            }
        }

        _stackManager.EnableByBooster();
    }
    #endregion Boost Swap

    #region Boost Refresh
    public void OnBoostRefresh()
    {
        _stackManager.ReGenerateStacks();
    }
    #endregion Boost Refresh

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
