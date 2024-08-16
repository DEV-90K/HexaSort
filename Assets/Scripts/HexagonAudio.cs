using Audio_System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonAudio : MonoBehaviour
{
    [SerializeField]
    private SoundData _SoundMerge;
    [SerializeField]
    private SoundData _SoundSort;

    public void PlayMerge()
    {
        SoundManager.Instance.CreateSoundBuilder()
            .WithPosition(transform.position)
            .WithRandomPitch()
            .Play(_SoundMerge);
    }

    public void PlaySort()
    {
        SoundManager.Instance.CreateSoundBuilder()
            .WithPosition(transform.position)
            .WithRandomPitch()
            .Play(_SoundSort);
    }

    internal void PlaySpawn()
    {
        throw new NotImplementedException();
    }
}
