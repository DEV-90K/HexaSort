using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonConfig : MonoBehaviour
{
    private Button _btn;

    private void Awake()
    {
        _btn = GetComponent<Button>();
    }

    private void Start()
    {
        _btn.onClick.AddListener(OnClickButton);
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveListener(OnClickButton);
    }

    private void OnClickButton()
    {
        PopupManager.Instance.ShowPopup<PopupLevelConfig>();
    }
}
