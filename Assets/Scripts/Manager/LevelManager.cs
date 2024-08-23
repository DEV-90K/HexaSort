using Audio_System;
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

    private LevelData _levelData = null;
    private LevelPresenterData _presenterData = null;
    private LevelConfig _config;

    private int amountHexagon = 0;

    private void OnEnable()
    {
        GameManager.OnChangeState += GameManager_OnChangeState;
    }

    private void OnDisable()
    {
        GameManager.OnChangeState -= GameManager_OnChangeState;
    }

    private void GameManager_OnChangeState(GameState state)
    {
        if(state == GameState.LEVEL_PLAYING)
        {
            _levelControl.OnPlaying();
        }
        else if(state == GameState.PAUSE)
        {
            _levelControl.OnPause();
        }
        else
        {
            _levelControl.OnIdle();
        }
    }

    private void Start()
    {
        LevelController.OnTurnCompleted += LevelController_OnTurnCompleted;
        _config = ResourceManager.instance.GetLevelConfig();
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

    public void OnInitCurrentLevel()
    {
        if(_levelData == null || _presenterData == null)
        {
            LoadLevelFromPlayer();
        }

        OnInit(_levelData, _presenterData);
    }

    public void OnInitLevelByID(int IDLevel)
    {
        _levelData = ResourceManager.instance.GetLevelByID(IDLevel);
        _presenterData = ResourceManager.instance.GetLevelPresenterDataByID(IDLevel);
        amountHexagon = 0;
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

    private void LoadLevelFromPlayer()
    {
        PlayerLevelData playerLevelData = MainPlayer.instance.GetPlayerLevelData().CopyObject();

        if(playerLevelData == null)
        {
            Debug.Log("Something wrong");
            return;
        }

        if (playerLevelData.Level == null)
        {
            _levelData = ResourceManager.instance.GetLevelByID(playerLevelData.IDLevel);
        }
        else
            _levelData = playerLevelData.Level;

        if (playerLevelData.LevelPresenter == null)
        {
            _presenterData = ResourceManager.instance.GetLevelPresenterDataByID(playerLevelData.IDLevel);
        }
        else
            _presenterData = playerLevelData.LevelPresenter;

        amountHexagon = playerLevelData.AmountCollected;
    }

    private void OnInit(LevelData levelData, LevelPresenterData presenterData)
    {
        _levelData = levelData;
        _presenterData = presenterData;
//        amountHexagon = 0;

        _gridManager.OnInit(_levelData.Grid);
        _stackManager.Configure(_presenterData.Amount, _presenterData.Probabilities);
        _stackManager.OnInit(_levelData.StackQueueData);
        _levelControl.OnInit();

        GUIManager.Instance.ShowScreen<ScreenLevel>(_presenterData);               
        MainPlayer.instance.CacheIDLevel(presenterData.Level);
    }

    public void OnReplay()
    {
        _gridManager.CollectGridImmediate();
        _stackManager.CollectRandomImmediate();
        amountHexagon = 0;

        OnInitCurrentLevel();
    }

    public void OnExit()
    {
        CacheCurrentLevelPlayingData();

        _gridManager.CollectGridImmediate();
        _stackManager.CollectRandomImmediate();
    }

    public void OnRevice()
    {
        StartCoroutine(IE_OnRevice());
    }

    private IEnumerator IE_OnRevice()
    {
        int amount = UnityEngine.Random.Range(1, 5);

        for(int i = 0; i < amount; i++)
        {
            yield return _levelControl.IE_RemoveRandomStack();
        }

        GameManager.instance.ChangeState(GameState.LEVEL_PLAYING);
    }

    private void OnFinish()
    {
        _gridManager.CollectOccupied();
        _stackManager.CollectRandomed();
        
    }

    private void OnFinishLosed()
    {
        //OnFinish();
        GameManager.Instance.ChangeState(GameState.FINISH);
        MainPlayer.instance.PlayingLosedLevel(_presenterData.Level);
        StartCoroutine(IE_FinishLosed(0f));
    }

    private IEnumerator IE_FinishLosed(float delay)
    {
        SFX_FinishLosed();
        yield return new WaitForSeconds(delay);        
        GUIManager.instance.ShowPopup<PopupLevelLosed>(_presenterData);
        //_levelData = null;
        //_presenterData = null;
    }

    private void SFX_FinishLosed()
    {
        SoundData data = SoundResource.instance.Failed;
        SoundManager.instance.CreateSoundBuilder()
            .Play(data);
    }

    private void OnFinishWoned()
    {
        GameManager.Instance.ChangeState(GameState.FINISH);
        OnFinish();
        MainPlayer.instance.PlayingWonedLevel(_presenterData.Level);        
        StartCoroutine(IE_FinishWoned(1f));
    }    

    private IEnumerator IE_FinishWoned(float delay)
    {
        SFX_FinishWoned();
        yield return new WaitForSeconds(delay);
        GUIManager.instance.ShowPopup<PopupLevelWoned>(_presenterData);
        _levelData = null;
        _presenterData = null;
    }

    private void SFX_FinishWoned()
    {
        SoundData sound = SoundResource.instance.Completed;

        SoundManager.instance.CreateSoundBuilder()
            .Play(sound);
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
        Vector3 hammerPos = stackHexagon.GetTopPosition();
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
                _levelControl.OnStackPlacedOnGridHexagon(grid);
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
