using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [SerializeField]
    private AudioSource _musicSource;
    [SerializeField]
    private AudioSource _soundsSource;
    [SerializeField]
    AudioClip musicClip;

    private void Start()
    {
        _musicVolume = 0.3f;
        _soundVolume = 0.5f;
        CanAudio = true;
    }

    private bool _canAudio;
    public bool CanAudio
    {
        get { return _canAudio; }
        set
        {
            _canAudio = value;
            CanMusic = value;
            CanSound = value;
        }
    }

    private bool _canMusic;
    public bool CanMusic
    {
        get { return (_canMusic); }
        set
        {
            _canMusic = value;
            if (CanMusic)
            {
                PlayMusic();
            }
            else
            {
                StopMusic();
            }

        }
    }

    private bool _canSound;
    public bool CanSound
    {
        get { return (_canSound); }
        set
        {
            _canSound = value;
        }
    }

    private float _soundVolume;
    public float SoundVolume
    {
        get { return _soundVolume; }
        set
        {
            _soundVolume = value;
        }
    }

    private float _musicVolume;
    public float MusicVolume
    {
        get { return _musicVolume; }
        set
        {
            _musicVolume = value;
        }
    }

    public void PlayMusic(AudioClip clip = null)
    {
        if (clip)
            _musicSource.clip = clip;
        else if (musicClip)
            _musicSource.clip = musicClip;
        else
            return;

        _musicSource.loop = true;
        _musicSource.volume = _musicVolume;

        if (_musicSource.clip && !_musicSource.isPlaying)
            _musicSource.Play();
    }

    public void PlayMusic(float vol)
    {
        _musicSource.volume = vol;
    }

    public void StopMusic()
    {
        _musicSource?.Stop();
    }

    public void PlaySound(AudioClip clip)
    {
        if (CanSound == false)
        {
            StopSound();
            return;
        }

        _soundsSource.clip = clip;
        _soundsSource.volume = _soundVolume;
        _soundsSource.Play();
    }

    public void PlaySound(AudioClip clip, bool loop = false)
    {
        if (CanSound == false)
        {
            StopSound();
            return;
        }

        _soundsSource.clip = clip;
        _soundsSource.volume = _soundVolume;
        _soundsSource.loop = loop;
        _soundsSource.Play();
    }

    public void PlaySound(AudioClip clip, float vol)
    {
        if (CanSound == false)
        {
            StopSound();
            return;
        }

        _soundsSource.clip = clip;
        _soundsSource.volume = vol;
        _soundsSource.Play();
    }

    //public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1)
    //{
    //    if (CanSound == false)
    //    {
    //        StopSound();
    //        return;
    //    }

    //    _soundsSource.transform.position = pos;
    //    PlaySound(clip, 0, vol);
    //}

    //public void PlaySound(AudioClip clip, float time = 0, float vol = 1, bool loop = false)
    //{
    //    if (CanSound == false)
    //    {
    //        StopSound();
    //        return;
    //    }

    //    _soundsSource.clip = clip;
    //    _soundsSource.time = time;
    //    _soundsSource.volume = vol;
    //    _soundsSource.loop = loop;
    //    _soundsSource.Play();
    //}

    public void StopSound()
    {
        _soundsSource?.Stop();
        _soundsSource.loop = false;
    }
}
