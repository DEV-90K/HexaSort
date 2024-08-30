using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupRevive : PopupBase
{
    [SerializeField]
    private TMP_Text _TxtRatio;
    [SerializeField]
    private TMP_Text _TxtCoin;
    [SerializeField]
    private Slider _Slider;

    [SerializeField]
    private Button _BtnRevive;
    [SerializeField]
    private Button _BtnAds;
    [SerializeField]
    private Button _BtnCancel;

    private int _currAmount;
    private LevelPresenterData _levelPresenterData;
    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);
        _levelPresenterData = (LevelPresenterData)paras[0];
        _currAmount = LevelManager.Instance.GetAmountHexagon();
    }

    public override void Show()
    {
        base.Show();

        _BtnRevive.gameObject.SetActive(MainPlayer.Instance.GetCoin() >= _levelPresenterData.CoinRevive);

        UpdateCoin();
        UpdateRatio();
        UpdateSlider();
    }

    private void UpdateRatio()
    {
        _TxtRatio.text = _currAmount + " / " + _levelPresenterData.Goal;
    }

    private void UpdateCoin()
    {
        _TxtCoin.text = _levelPresenterData.CoinRevive.ToString();
    }

    private void UpdateSlider()
    {
        _Slider.minValue = 0;
        _Slider.maxValue = _levelPresenterData.Goal;
        _Slider.value = _currAmount;
    }

    private void Start()
    {
        _BtnRevive.onClick.AddListener(OnClickRevive);
        _BtnAds.onClick.AddListener(OnClickAds);
        _BtnCancel.onClick.AddListener(OnClickCancle);
    }

    private void OnDestroy()
    {
        _BtnRevive.onClick.RemoveListener(OnClickRevive);
        _BtnAds.onClick.RemoveListener(OnClickAds);
        _BtnCancel.onClick.RemoveListener(OnClickCancle);
    }

    private void OnClickRevive()
    {
        MainPlayer.Instance.SubCoin(_levelPresenterData.CoinRevive);
        LevelManager.Instance.OnRevice();
        GUIManager.Instance.HidePopup<PopupLevelLosed>();
        Hide();
    }

    private void OnClickAds()
    {
        LevelManager.Instance.OnRevice();
        GUIManager.Instance.HidePopup<PopupLevelLosed>();
        Hide();
    }

    private void OnClickCancle()
    {
        Hide();
    }
}
