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
        UpdateRatio(0);
        UpdateSlider(0);

        StartCoroutine(IE_UpdateSlider());
    }

    private void UpdateRatio(int amount)
    {
        _TxtRatio.text = amount + " / " + _levelPresenterData.Goal;
    }

    private void UpdateCoin()
    {
        _TxtCoin.text = _levelPresenterData.CoinRevive.ToString();
    }

    private IEnumerator IE_UpdateSlider()
    {
        yield return new WaitForSeconds(0.2f); //Time tween show
        for(int i = 0; i <= _currAmount; i++)
        {
            UpdateSlider(i);
            UpdateRatio(i);
            yield return null;
        }
    }

    private void UpdateSlider(int amount)
    {
        _Slider.minValue = 0;
        _Slider.maxValue = _levelPresenterData.Goal;
        _Slider.value = amount;
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
