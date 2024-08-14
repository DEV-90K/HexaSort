using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField]
    private AudioSystem _AudioSystem;

    [SerializeField]
    private AudioClip _SoundSetting;
    [SerializeField]
    private AudioClip _SoundPlaced;
    [SerializeField]
    private AudioClip _SoundVanishSpecial;
    [SerializeField]
    private AudioClip _SoundShowPopup;
    [SerializeField]
    private AudioClip _SoundHidePopup;
    [SerializeField]
    private AudioClip _SoundReached;
    [SerializeField]
    private AudioClip _SoundNoticeVanish;
    [SerializeField]
    private AudioClip _SoundBoost;
    [SerializeField]
    private AudioClip _SoundWin;
    [SerializeField]
    private AudioClip _SoundOpenChest;

    public void PlaySoundOpenChest()
    {
        _AudioSystem.PlaySound(_SoundOpenChest);
    }

    public void PlayeSoundWin()
    {
        _AudioSystem.PlaySound(_SoundWin);
    }

    public void PlaySoundBoost()
    {
        _AudioSystem.PlaySound(_SoundBoost);
    }

    public void PlaySoundNoticeVanish()
    {
        _AudioSystem.PlaySound(_SoundNoticeVanish);
    }

    public void PlaySoundReached()
    {
        _AudioSystem.PlaySound(_SoundReached);
    }

    public void PlaySoundHidePopup()
    {
        _AudioSystem.PlaySound(_SoundHidePopup);
    }

    public void PlaySoundShowPopup()
    {
        _AudioSystem.PlaySound(_SoundShowPopup);
    }

    public void PlaySoundVanishSpecial()
    {
        _AudioSystem.PlaySound(_SoundVanishSpecial);
    }

    public void PlaySoundPlaced()
    {
        _AudioSystem.PlaySound(_SoundPlaced);
    }
    public void PlaySoundSetting(float vol)
    {
        _AudioSystem.SoundVolume = vol;
        _AudioSystem.PlaySound(_SoundSetting);
    }

    public void StopLoopSound()
    {
        _AudioSystem.StopSound();
    }

    public float GetSoundVolume()
    {
        return _AudioSystem.SoundVolume;
    }

    public bool CanSound()
    {
        return _AudioSystem.CanSound;
    }

    public float GetMusicVolume()
    {
        return _AudioSystem.MusicVolume;
    }

    public bool CanMusic()
    {
        return _AudioSystem.CanMusic;
    }

    public void PlayMusicSetting(float vol)
    {
        _AudioSystem.MusicVolume = vol;
        _AudioSystem.PlayMusic(vol);
    }

    public void UpdateMusicState(bool toggle)
    {
        _AudioSystem.CanMusic = toggle;
    }

    public void UpdateSoundState(bool toggle)
    {
        _AudioSystem.CanSound = toggle;
    }
}
