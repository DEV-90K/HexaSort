using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource _AudioSource;
    [SerializeField]
    private AudioClip _Merge;
    [SerializeField]
    private AudioClip _Sort;
    [SerializeField]
    private AudioClip _Spawn;

    public void PlayMerge()
    {
        _AudioSource.clip = _Merge;
        _AudioSource.Play();
    }

    public void PlaySort()
    {
        _AudioSource.clip = _Sort;
        _AudioSource.Play();
    }

    public void PlaySpawn()
    {
        _AudioSource.clip = _Spawn;
        _AudioSource.Play();
    }

    public void StopAudio()
    {
        _AudioSource.Stop();
    }
}
