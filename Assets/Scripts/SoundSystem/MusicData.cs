using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio_System
{
    [Serializable]
    public class MusicData
    {
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        public bool loop;        
        public bool playOnAwake;

        public float volume = 1;
        public bool bypassListenerEffects = true;
    }
}
