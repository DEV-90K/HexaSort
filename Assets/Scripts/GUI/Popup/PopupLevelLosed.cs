using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupLevelLosed : PopupBase
{
    [SerializeField]
    private Button _BtnHome;
    [SerializeField]
    private Button _BtnReplay;
    [SerializeField]
    private Button _BtnRevice;

    private LevelPresenterData _presenterData;

    public void OnInit(LevelPresenterData data)
    {
        _presenterData = data;
    }

    public override void Show()
    {
        base.Show();
        GameManager.Instance.ChangeState(GameState.PAUSE);
    }

    private void Start()
    {
        _BtnHome.onClick.AddListener(OnClickBtnHome);
        _BtnReplay.onClick.AddListener(OnClickBtnReplay);
        _BtnRevice.onClick.AddListener(OnClickBtnRevice);
    }

    private void OnDestroy()
    {
        _BtnHome.onClick.RemoveListener(OnClickBtnHome);
        _BtnReplay.onClick.RemoveListener(OnClickBtnReplay);
        _BtnRevice.onClick.RemoveListener(OnClickBtnRevice);
    }

    private void OnClickBtnHome()
    {
        GameManager.Instance.ChangeState(GameState.FINISH);
        GUIManager.Instance.HideScreen<ScreenLevel>();
        GUIManager.Instance.ShowScreen<ScreenMain>();
        PopupManager.Instance.HidePopup<PopupLevelLosed>();
    }

    private void OnClickBtnReplay()
    {
        LevelManager.Instance.OnReplay();
        PopupManager.Instance.HidePopup<PopupLevelLosed>();
    }

    private void OnClickBtnRevice()
    {
        LevelManager.Instance.OnRevice();
        PopupManager.Instance.HidePopup<PopupLevelLosed>();
    }
}
