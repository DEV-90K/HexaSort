using Audio_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSetting : MonoBehaviour
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
        SFX_ClickSetting();
        GUIManager.Instance.ShowPopup<PopupSetting>();
    }

    private void SFX_ClickSetting()
    {
        SoundData soundData = SoundResource.Instance.ButtonClick;
        SoundManager.Instance.CreateSoundBuilder().WithRandomPitch().Play(soundData);
    }
}