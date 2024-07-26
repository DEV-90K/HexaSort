using System;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField]
    private GridManager _gridManager;
    [SerializeField]
    private StackManager _stackManager;
    [SerializeField]
    private LevelController _levelControl;
    [SerializeField]
    private Hammer _hammer;

    private LevelData _levelData;
    private LevelPresenterData _presenterData;
    private LevelConfig _config;

    private int amountHexagon = 0;

    private void Start()
    {
        LevelController.OnTurnCompleted += LevelController_OnTurnCompleted;

        _config = ResourceManager.instance.GetLevelConfig();

        PlayerLevelData playerLevelData = MainPlayer.instance.GetPlayerLevelData().CopyObject();
        if(playerLevelData != null)
        {
            if(playerLevelData.Level == null)
            {
                _levelData = ResourceManager.instance.GetLevelByID(playerLevelData.IDLevel);
            }
            else
                _levelData = playerLevelData.Level;

            if(playerLevelData.LevelPresenter == null)
            {
                ResourceManager.instance.GetLevelPresenterDataByID(playerLevelData.IDLevel);
            }
            else
                _presenterData = playerLevelData.LevelPresenter;
        }
        else
        {
            _levelData = ResourceManager.instance.GetLevelByID(playerLevelData.IDLevel);
            _presenterData = ResourceManager.instance.GetLevelPresenterDataByID(playerLevelData.IDLevel);
        }

        _hammer.gameObject.SetActive(false);
        _levelControl.OnSetup(_config.SpaceSpecialEffects);
    }

    private void OnDestroy()
    {
        LevelController.OnTurnCompleted -= LevelController_OnTurnCompleted;
    }

    public int GetAmountHexagon()
    {
        return amountHexagon;
    }

    public void UpdateAmountHexagon(int amount)
    {
        amountHexagon = amount;
    }
    
    public void OnInitLevelByID(int IDLevel)
    {
        LevelData levelData = ResourceManager.instance.GetLevelByID(IDLevel);
        LevelPresenterData presenterData = ResourceManager.instance.GetLevelPresenterDataByID(IDLevel);

        OnInit(levelData, presenterData);
    }

    public void OnInitNextLevel()
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

    public void OnInitCurrentLevel()
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
        _gridManager.OnInit(_levelData.Grid);

        _stackManager.Configure(_presenterData.Amount, _presenterData.Probabilities);
        _stackManager.OnInit(_levelData.StackQueueData);

        MainPlayer.instance.CacheIDLevel(presenterData.Level);
        GUIManager.Instance.ShowScreen<ScreenLevel>(_presenterData);       
    }

    public void OnReplay()
    {
        _gridManager.CollectGridImmediate();
        _stackManager.CollectRandomImmediate();
        OnInitCurrentLevel();
    }

    public void OnExit()
    {
        //TODO: Save Level Data To Player 
        CacheCurrentLevelPlayingData();

        _gridManager.CollectGridImmediate();
        _stackManager.CollectRandomImmediate();

        Debug.Log("Run Here");
        GUIManager.instance.ShowScreen<ScreenMain>();
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
        GameManager.Instance.ChangeState(GameState.PAUSE);
        StartCoroutine(IE_FinishLosed(1f));
    }

    private IEnumerator IE_FinishLosed(float delay)
    {
        yield return new WaitForSeconds(delay);
        GUIManager.instance.ShowPopup<PopupLevelLosed>(_presenterData);
    }

    private void OnFinishWoned()
    {
        OnFinish();
        GameManager.Instance.ChangeState(GameState.PAUSE);
        StartCoroutine(IE_FinishWoned(1f));
    }

    private IEnumerator IE_FinishWoned(float delay)
    {
        yield return new WaitForSeconds(delay);
        GUIManager.instance.ShowPopup<PopupLevelWoned>(_presenterData);
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
        MainPlayer.instance.SubHammer(1);
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

    public void OnBoostSwap(GridHexagon[] grids)
    {
        MainPlayer.instance.SubSwap(1);
        foreach (GridHexagon grid in grids)
        {
            _levelControl.OnStackPlacedOnGridHexagon(grid);
        }
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
        MainPlayer.instance.SubRefresh(1);
        _stackManager.ReGenerateStacks();
    }
    #endregion Boost Refresh

    private void LevelController_OnTurnCompleted()
    {

        if (amountHexagon >= _presenterData.Goal)
        {
            OnFinishWoned();
        }
        else if (!_gridManager.CheckEmptyGrid())
        {
            OnFinishLosed();
        }
        else
        {
            onStackMergeCompleted();
        }
    }

    private void onStackMergeCompleted()
    {
        GridHexagon[] gridHexagons = _gridManager.GetGridHexagonsUnLockByHexagon(amountHexagon);

        if (gridHexagons.Length > 0)
        {
            foreach (GridHexagon grid in gridHexagons)
            {
                _stackManager.MergeStackIntoGrid(grid);
            }
        }
    }

    #region Level Data
    public void CacheCurrentLevelPlayingData()
    {
        _levelData = GetCurrentLevelPlayingData();
    }

    public LevelPresenterData GetPresenterData()
    {
        return _presenterData;
    }

    public LevelData GetLevelData()
    {
        return _levelData;
    }

    internal LevelData GetCurrentLevelPlayingData()
    {
        GridHexagonData[] gridHexagonDatas =  _gridManager.GetCurrentGridPlayingData();
        GridData gridData = new GridData(gridHexagonDatas);
        StackQueueData stackQueueData = _levelData.StackQueueData.CopyObject();
        LevelData data = new LevelData(gridData, stackQueueData);
        return data;
    }
    #endregion Level Data
}
