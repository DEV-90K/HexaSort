using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenLevel : ScreenBase, IBoostHammer, IBoostSwap
{
    [SerializeField]
    private TMP_Text txtRatio;
    [SerializeField]
    private Image imgFill;

    [SerializeField]
    private ButtonBoostHammer btnBoostHammer;
    [SerializeField]
    private ButtonBoostSwap btnBoostSwap;

    private LevelPresenterData presenterData;
    private int amount = 0;

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

    public void OnBoostSwap(GridHexagon grid)
    {
        LevelManager.Instance.OnBoostSwap(grid);
    }

    public void ExitBoostSwap()
    {
        LevelManager.Instance.ExitBoostSwap();
    }
    #endregion Boost Swap

    protected override void Awake()
    {
        base.Awake();
        btnBoostHammer.OnInit(this);
        btnBoostSwap.OnInit(this);
    }

    public override void OnInit(params object[] paras)
    {
        base.OnInit(paras);

        if (paras.Length > 1)
            presenterData = (LevelPresenterData)paras[0];
        else
            presenterData = LevelManager.Instance.GetPresenterData();

        amount = LevelManager.Instance.GetAmountHexagon();

        UpdateTxtRatio(amount);
        UpdateImgFill(amount/(float)presenterData.Goal);

        Show();        
    }

    public override void Show()
    {
        base.Show();
        GameManager.Instance.ChangeState(GameState.PLAYING);
    }

    private void UpdateTxtRatio(int amount)
    {
        txtRatio.text = $"{Mathf.Min(amount, presenterData.Goal)}/{presenterData.Goal}";        
    }

    private void UpdateImgFill(float ratio)
    {
        imgFill.fillAmount = Mathf.Min(ratio, 1);
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
