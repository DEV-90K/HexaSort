using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBoostRefresh : MonoBehaviour
{
    private Button mButton;
    private void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        LevelManager.Instance.OnBoostRefresh();
    }
}
