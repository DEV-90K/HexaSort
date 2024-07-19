using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBoostSwap : MonoBehaviour
{
    private Button mButton;
    private IBoostSwap _able;
    private void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(OnClickButton);
    }

    public void OnInit(IBoostSwap able)
    {
        _able = able;
    }

    private void OnClickButton()
    {
        GUIManager.Instance.HideScreen<ScreenLevel>();
        GUIManager.Instance.ShowPopup<PopupBoostSwap>(_able);
    }
}
