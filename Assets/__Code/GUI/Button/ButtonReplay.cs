using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonReplay : MonoBehaviour
{
    private Button _btn;

    private void Awake()
    {
        _btn = GetComponent<Button>();
    }

    private void Start()
    {
        _btn.onClick.AddListener(OnClickBtnReplay);
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveListener(OnClickBtnReplay);
    }

    private void OnClickBtnReplay()
    {
        LevelManager.Instance.OnReplay();
    }
}
