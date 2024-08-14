using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : PopupBase
{
    private MusicSetting _musicSetting;
    private SoundSetting _soundSetting;

    private void Awake()
    {
        _musicSetting = GetComponent<MusicSetting>();
        _soundSetting = GetComponent<SoundSetting>();
    }    
}
