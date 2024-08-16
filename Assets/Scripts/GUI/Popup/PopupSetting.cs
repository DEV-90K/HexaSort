using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : PopupBase
{
    private MusicSetting _musicSetting;
    private SoundSetting _soundSetting;

    private PlayerAudioData _audioData;

    private void Awake()
    {
        _musicSetting = GetComponent<MusicSetting>();
        _soundSetting = GetComponent<SoundSetting>();
    }

    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);

        _audioData = MainPlayer.Instance.GetAudioData();
        _musicSetting.OnInit(_audioData.MusicVol, _audioData.CanMusic);
        _soundSetting.OnInit(_audioData.SoundVol, _audioData.CanSound);
    }

    public void OnClickClose()
    {
        _audioData.SoundVol = _soundSetting.GetVol();
        _audioData.CanSound = _soundSetting.CheckCanSound();

        _audioData.MusicVol = _musicSetting.GetVol();
        _audioData.CanMusic = _musicSetting.CheckCanMusic();

        MainPlayer.Instance.UpdateAudioData(_audioData);
        Hide();
    }
}
