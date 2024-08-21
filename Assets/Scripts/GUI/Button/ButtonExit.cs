using Audio_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonExit : MonoBehaviour
{
    private Button _btnExit;

    private void Awake()
    {
        _btnExit = GetComponent<Button>();
    }

    private void Start()
    {
        _btnExit.onClick.AddListener(OnClickExit);
    }

    private void OnDestroy()
    {
        _btnExit.onClick.RemoveListener(OnClickExit);
    }

    public virtual void OnClickExit()
    {
        SFX_ClickExit();
        LevelManager.Instance.OnExit();
        GUIManager.Instance.HideScreen<ScreenLevel>();
    }

    public void SFX_ClickExit()
    {
        SoundData soundData = SoundResource.Instance.ButtonClick;
        SoundManager.Instance.CreateSoundBuilder().WithRandomPitch().Play(soundData);
    }
}
