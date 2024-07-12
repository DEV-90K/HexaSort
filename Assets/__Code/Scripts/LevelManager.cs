using System.Collections;
using UnityEngine;
using Mul21_Lib;
using System;
using System.Collections.Generic;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField]
    private GridManager _gridManager;
    [SerializeField]
    private StackManager _stackManager;

    [SerializeField]
    private Hammer _hammer;

    private LevelData _levelData;
    private LevelPresenterData _presenterData;

    private int amountHexagon = 0;

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
        _hammer.gameObject.SetActive(false);
        OnInitLevelByID(1);
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
    
    private void OnInitLevelByID(int IDLevel)
    {
        LevelData levelData = ResourceManager.instance.GetLevelByID(IDLevel);
        LevelPresenterData presenterData = ResourceManager.instance.GetLevelPresenterDataByID(IDLevel);

        OnInit(levelData, presenterData);
    }

    private void OnInitNextLevel()
    {
        int IDLevel = _presenterData.Level;
        LevelData levelData = ResourceManager.instance.GetLevelByID(IDLevel + 1);
        LevelPresenterData presenterData = ResourceManager.instance.GetLevelPresenterDataByID(IDLevel + 1);

        if(levelData == null || presenterData == null)
        {
            levelData = _levelData;
            presenterData = _presenterData;
        }

        OnInit(levelData, presenterData);
    }

    private void OnInitCurrentLevel()
    {
        OnInit(_levelData, _presenterData);
    }

    public void UpdateCurrentLevel(LevelPresenterData levelPresenter)
    {
        int IDLevel = levelPresenter.Level;
        LevelData levelData = ResourceManager.instance.GetLevelByID(IDLevel);
        LevelPresenterData presenterData = levelPresenter;

        OnFinish();
        this.Invoke(() => OnInit(levelData, presenterData), 1f);
    }

    private void OnInit(LevelData levelData, LevelPresenterData presenterData)
    {
        _levelData = levelData;
        _presenterData = presenterData;

        amountHexagon = 0;

        _gridManager.OnInit(levelData.Grid);
        _stackManager.OnInit(levelData.StackQueueData);

        GUIManager.Instance.ShowScreen<ScreenLevel>(presenterData);       
    }

    public void OnReplay()
    {
        _gridManager.CollectGridImmediate();
        _stackManager.CollectRandomImmediate();
        OnInitCurrentLevel();
    }

    private void OnFinish()
    {
        GameManager.instance.ChangeState(GameState.FINISH);
        _gridManager.CollectOccupied();
        _stackManager.CollectRandomed();
    }

    private void OnFinishLosed()
    {
        OnFinish();
        this.Invoke(() => OnInitCurrentLevel(), 1f);
    }

    private void OnFinishWoned()
    {
        OnFinish();
        this.Invoke(() => OnInitNextLevel(), 1f);
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

        if (amountHexagon >= _presenterData.Goal)
        {
            OnFinishWoned();
        }
        else if (!_gridManager.CheckEmptyGrid())
        {
            OnFinishLosed();
        }
    }

    #region Level Data
    internal LevelData GetCurrentLevelPlayingData()
    {
        GridHexagonData[] gridHexagonDatas =  _gridManager.GetCurrentGridPlayingData();
        GridData gridData = new GridData(gridHexagonDatas);

        _levelData.UpdateGridData(gridData);

        return _levelData;
    }

    internal int GetCurrentLevelPlayingID()
    {
        return _presenterData.Level;
    }
    #endregion Level Data

}
