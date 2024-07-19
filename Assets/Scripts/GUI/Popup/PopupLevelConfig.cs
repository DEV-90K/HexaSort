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

    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);
        _presenterData = LevelManager.Instance.GetPresenterData();
        UpdateContentHexagonInput();
        UpdateContentLevelInput();
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
        LevelPresenterData presenterData = _presenterData.CopyObject();
        presenterData.UpdateLevel(GetContentLevelInput());
        presenterData.UpdateGoal(GetContentHexagonInput());

        LevelManager.Instance.UpdateCurrentLevel(presenterData);
        Hide();
    }

    private void OnClickButtonClose()
    {
        Hide();
    }
}
