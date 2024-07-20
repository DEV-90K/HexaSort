using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMain : ScreenBase
{
    [SerializeField]
    private Button _BtnPlay;
    [SerializeField]
    private Button _BtnGrallery_1;
    [SerializeField]
    private Button _BtnGrallery_2;
    [SerializeField]
    private Button _BtnGrallery_3;
    [SerializeField]
    private TMP_Text _txtCoin;
    [SerializeField]
    private TMP_Text _txtMaterial;

    private int _coin;
    private int _material;

    public override void OnInit(params object[] paras)
    {
        base.OnInit(paras);
        _coin = MainPlayer.Instance.GetCoin();
        _material = MainPlayer.Instance.GetMaterial();
    }

    public override void Show()
    {
        base.Show();
        UpdateTxtCoin();
        UpdateTxtMaterial();
    }

    private void Start()
    {
        _BtnPlay.onClick.AddListener(OnClickBtnPlay);
        _BtnGrallery_1.onClick.AddListener(OnClickGrallery_1);
        _BtnGrallery_2.onClick.AddListener(OnClickGrallery_2);
        _BtnGrallery_3.onClick.AddListener(OnClickGrallery_3);
    }

    private void OnDestroy()
    {
        _BtnPlay.onClick.RemoveListener(OnClickBtnPlay);
        _BtnGrallery_1.onClick.RemoveListener(OnClickGrallery_1);
        _BtnGrallery_2.onClick.RemoveListener(OnClickGrallery_2);
        _BtnGrallery_3.onClick.RemoveListener(OnClickGrallery_3);
    }

    private void OnClickBtnPlay()
    {
        LevelManager.Instance.OnInitCurrentLevel();
        Hide();
    }

    private void OnClickGrallery_1()
    {
        GUIManager.Instance.ShowPopup<PopupGallery>(1);
    }

    private void OnClickGrallery_2()
    {
        GUIManager.Instance.ShowPopup<PopupGallery>(2);
    }

    private void OnClickGrallery_3()
    {
        GUIManager.Instance.ShowPopup<PopupGallery>(3);
    }

    private void UpdateTxtCoin()
    {
        _txtCoin.text = _coin.ToString();
    }

    private void UpdateTxtMaterial()
    {
        _txtMaterial.text = _material.ToString();
    }
}
