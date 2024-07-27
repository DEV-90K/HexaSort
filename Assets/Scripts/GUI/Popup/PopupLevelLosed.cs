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
    }

    private void OnDestroy()
    {
        _BtnHome.onClick.RemoveListener(OnClickBtnHome);
        _BtnReplay.onClick.RemoveListener(OnClickBtnReplay);
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
        LevelManager.Instance.OnInitLevelByID(_presenterData.Level);
        Hide();
    }
}
