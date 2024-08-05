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

    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);
        _presenterData = (LevelPresenterData) paras[0];
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
        Hide();
    }

    private void OnClickBtnReplay()
    {
        LevelManager.Instance.OnReplay();
        Hide();
    }

    private void OnClickBtnRevice()
    {
        LevelManager.Instance.OnRevice();
        Hide();
    }
}
