using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupLevelConfig : PopupBase
{
    [SerializeField]
    private TMP_InputField _HexagonInput;
    [SerializeField]
    private TMP_InputField _LevelInput;
    [SerializeField]
    private Button _BtnSave;
    [SerializeField]
    private Button _BtnClose;

    private LevelPresenterData _presenterData;

    public void OnInit()
    {
        _presenterData = LevelManager.Instance.GetPresenterData();
        UpdateContentHexagonInput();
        UpdateContentLevelInput();
    }

    public override void Show()
    {
        base.Show();
        GameManager.Instance.ChangeState(GameState.PAUSE);
    }

    private void Start()
    {
        _BtnSave.onClick.AddListener(OnClickButtonSave);
        _BtnClose.onClick.AddListener(OnClickButtonClose);
    }

    private void OnDestroy()
    {
        _BtnSave.onClick.RemoveListener(OnClickButtonSave);
        _BtnClose.onClick.RemoveListener(OnClickButtonClose);
    }

    private void UpdateContentHexagonInput()
    {
        int numberOfHexagon = _presenterData.Goal;
        _HexagonInput.text = numberOfHexagon.ToString();
    }

    private int GetContentHexagonInput()
    {
        string val = _HexagonInput.text.Trim();

        if(string.IsNullOrEmpty(val))
        {
            Debug.Log("Wrong when set amount Hexagon");
        }

        return Int32.Parse(val);
    }

    private void UpdateContentLevelInput()
    {
        int idLevel = _presenterData.Level;
        _LevelInput.text = idLevel.ToString();
    }

    private int GetContentLevelInput()
    {
        string val = _LevelInput.text.Trim();

        if (string.IsNullOrEmpty(val))
        {
            Debug.Log("Wrong when set Level ID");
        }

        return Int32.Parse(val);
    }

    private void OnClickButtonSave()
    {
        GameManager.Instance.ChangeState(GameState.FINISH);
        LevelPresenterData presenterData = _presenterData.CopyObject();
        presenterData.UpdateLevel(GetContentLevelInput());
        presenterData.UpdateGoal(GetContentHexagonInput());

        LevelManager.Instance.UpdateCurrentLevel(presenterData);
        PopupManager.Instance.HidePopup<PopupLevelConfig>();
    }

    private void OnClickButtonClose()
    {
        GameManager.Instance.ChangeState(GameState.LEVEL_PLAYING);
        PopupManager.Instance.HidePopup<PopupLevelConfig>();
    }
}
