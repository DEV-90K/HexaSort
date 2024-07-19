using System.Collections;
using System.Collections.Generic;
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
}