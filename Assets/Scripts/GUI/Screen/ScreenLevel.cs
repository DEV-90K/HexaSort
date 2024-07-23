using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenLevel : ScreenBase, IBoostHammer, IBoostSwap
{
    [SerializeField]
    private TMP_Text _txtRatio;
    [SerializeField]
    private TMP_Text _txtCoin;
    [SerializeField]
    private Image _imgFill;

    [SerializeField]
    private ButtonBoostHammer _btnBoostHammer;
    [SerializeField]
    private ButtonBoostSwap _btnBoostSwap;
    [SerializeField]
    private ButtonBoostRefresh _btnBoostRefresh;

    private LevelPresenterData presenterData;
    private int amount = 0;

    public override void OnInit(params object[] paras)
    {
        base.OnInit(paras);
        presenterData = (LevelPresenterData)paras[0];
        Show();
    }

    public override void Show()
    {
        base.Show();
        amount = LevelManager.Instance.GetAmountHexagon();
        UpdateTxtRatio(amount);
        UpdateImgFill(amount / (float)presenterData.Goal);
        UpdateTxtPlayerCoin();
        GameManager.Instance.ChangeState(GameState.LEVEL_PLAYING);
    }

    public override void Hide()
    {
        base.Hide();
        GameManager.Instance.ChangeState(GameState.FINISH);
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

    private void UpdateImgFill(float ratio)
    {
        _imgFill.fillAmount = Mathf.Min(ratio, 1);
    }

    private void UpdateTxtPlayerCoin()
    {
        _txtCoin.text = MainPlayer.Instance.GetCoin().ToString();
    }

    private void Hexagon_OnVanish()
    {
        if(GameManager.Instance.IsState(GameState.FINISH))
        {
            return;
        }

        amount++;
        LevelManager.Instance.UpdateAmountHexagon(amount);
        UpdateTxtRatio(amount);
        UpdateImgFill(amount / (float)presenterData.Goal);
    }
}
