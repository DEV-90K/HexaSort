using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using System;

public class ScreenLevel : ScreenBase
{
    [SerializeField]
    private TMP_Text txtRatio;
    [SerializeField]
    private Image imgFill;
    [SerializeField]
    private Button btnReplay;

    private LevelPresenterData presenterData;

    public void OnChangeHexagon(int amount)
    {
        Debug.Log("OnChangeHexagon: " + amount);
        UpdateTxtRatio(amount);
        UpdateImgFill((float) amount / (float) presenterData.Goal);
    }

    protected override void Awake()
    {
        base.Awake();
        btnReplay.onClick.AddListener(OnBtnReplayClick);
    }

    public override void OnInit(params object[] paras)
    {
        base.OnInit(paras);
        presenterData = (LevelPresenterData) paras[0];

        UpdateTxtRatio(0);
        UpdateImgFill(0);

        Show();
    }

    private void UpdateTxtRatio(int amount)
    {
        txtRatio.text = $"{Mathf.Min(amount, presenterData.Goal)}/{presenterData.Goal}";        
    }

    private void UpdateImgFill(float ratio)
    {
        imgFill.fillAmount = Mathf.Min(ratio, 1);
    }

    private void OnBtnReplayClick()
    {
        LevelManager.Instance.OnReplay();
    }
}
