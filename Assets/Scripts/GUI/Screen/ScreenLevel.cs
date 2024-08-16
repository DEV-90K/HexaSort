using Audio_System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IBoostTrick
{
    public void ShowBoostTrick();
    public void HideBoostTrick();
}

public class ScreenLevel : ScreenBase, IBoostHammer, IBoostSwap
{
    [SerializeField]
    private MusicData musicData;

    [SerializeField]
    private TMP_Text _txtRatio;
    [SerializeField]
    private TMP_Text _txtLevel;
    [SerializeField]
    private Slider _imgFill;

    [SerializeField]
    private ButtonBoostHammer _btnBoostHammer;
    [SerializeField]
    private ButtonBoostSwap _btnBoostSwap;
    [SerializeField]
    private ButtonBoostRefresh _btnBoostRefresh;

    private LevelPresenterData presenterData;
    private int amount = 0;

    private IBoostTrick _IBoostTrick;

    public override void OnInit(params object[] paras)
    {
        base.OnInit(paras);
        presenterData = (LevelPresenterData)paras[0];
        Show();
    }

    public override void Show()
    {
        base.Show();
        MusicManager.Instance.Play(musicData);
        GameManager.Instance.ChangeState(GameState.LEVEL_PLAYING);

        amount = LevelManager.Instance.GetAmountHexagon();
        UpdateTxtRatio(amount);
        UpdateImgFill(amount / (float)presenterData.Goal);
        UpdateTxtLevel();
    }

    public override void Hide()
    {
        base.Hide();
        
        if(GameManager.Instance.IsState(GameState.LEVEL_PLAYING))
        {
            GameManager.Instance.ChangeState(GameState.FINISH);
        }
    }

    public void ShowBoostTrick()
    {
        int idx = UnityEngine.Random.Range(0, 3);

        if(idx == 0)
        {
            _IBoostTrick = _btnBoostHammer as IBoostTrick;
        }
        else if(idx == 1)
        {
            _IBoostTrick = _btnBoostSwap as IBoostTrick;
        }
        else if(idx == 2)
        {
            _IBoostTrick = _btnBoostRefresh as IBoostTrick;
        }

        _IBoostTrick.ShowBoostTrick();
    }

    public void HideBoostTrick()
    {
        Debug.Log("Screen Level Hide Boost Trick");
        _IBoostTrick?.HideBoostTrick();
    }

    private void OnEnable()
    {
        Hexagon.OnVanish += Hexagon_OnVanish;
    }

    private void OnDisable()
    {
        Hexagon.OnVanish -= Hexagon_OnVanish;
    }

    #region Boost Hammer
    public void EnterBoostHammer()
    {
        LevelManager.Instance.EnterBoostHammer();
    } 

    public void OnBoostHammer(Hexagon hexagon)
    {
        LevelManager.Instance.OnBoostHammer(hexagon);
    }
    
    public void ExitBoostHammer()
    {
        LevelManager.Instance.ExitBoostHammer();
    }
    #endregion Boost Hammer

    #region Boost Swap
    public void EnterBoostSwap()
    {
        LevelManager.Instance.EnterBoostSwap();
    }

    public void OnBoostSwap(GridHexagon[] grids)
    {
        LevelManager.Instance.OnBoostSwap(grids);
    }

    public void ExitBoostSwap()
    {
        LevelManager.Instance.ExitBoostSwap();
    }
    #endregion Boost Swap

    private void Start()
    {
        _btnBoostHammer.OnInit(this);
        _btnBoostSwap.OnInit(this);
        _btnBoostRefresh.OnInit();
    }

    private void UpdateTxtRatio(int amount)
    {
        _txtRatio.text = $"{Mathf.Min(amount, presenterData.Goal)}/{presenterData.Goal}";        
    }
    private void UpdateTxtLevel()
    {
        _txtLevel.text = $"Level {presenterData.Level}";
    }

    private void UpdateImgFill(float ratio)
    {
        _imgFill.value = Mathf.Min(ratio, 1);
    }

    private void Hexagon_OnVanish()
    {
        if(GameManager.Instance.IsState(GameState.LEVEL_PLAYING))
        {
            amount++;
            LevelManager.Instance.UpdateAmountHexagon(amount);
            UpdateTxtRatio(amount);
            UpdateImgFill(amount / (float)presenterData.Goal);
        }
    }
}
