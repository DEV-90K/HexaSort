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

    private void Start()
    {
        _BtnPlay.onClick.AddListener(OnClickBtnPlay);
        _BtnGrallery_1.onClick.AddListener(OnClickGrallery_1);
    }

    private void OnDestroy()
    {
        _BtnPlay.onClick.RemoveListener(OnClickBtnPlay);
        _BtnGrallery_1.onClick.AddListener(OnClickGrallery_1);
    }

    private void OnClickBtnPlay()
    {        
        LevelManager.Instance.OnInitCurrentLevel();
    }

    private void OnClickGrallery_1()
    {
        GUIManager.Instance.ShowPopup<PopupGallery>();
    }
}
