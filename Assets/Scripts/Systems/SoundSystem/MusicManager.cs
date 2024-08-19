
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio_System
{
    public class MusicManager : MonoSingleton<MusicManager>
    {
        [SerializeField] List<MusicData> initialMusicList;

        private const float crossFadeTime = 1.0f;
        private float fading = 0f;
        private float vanishFading = 0f;

        private MusicData musicData = null;
        private AudioSource current = null;

        private readonly Queue<MusicData> playlist = new();
        Coroutine coroutinePlay = null;

        private void Start()
        {
            PlayerAudioData audioData = MainPlayer.Instance.GetAudioData();
            canMusic = audioData.CanMusic;
            Volumne(audioData.MusicVol);
        }

        public void PlayNextTrack()
        {
            if (playlist.TryDequeue(out MusicData nextTrack))
            {
                Play(nextTrack);
            }
        }

        public void Play(MusicData data)
        {
            //if (current && current.clip == data.clip)
            //{
            //    return;
            //}

            if(current && current.isPlaying && current.clip == data.clip)
            {
                return;
            }

            if (!CanMusic) 
                return;

            vanishFading = 0.001f;
            fading = 0.0f;

            if(coroutinePlay != null) 
                StopCoroutine(coroutinePlay);

            coroutinePlay = StartCoroutine(IE_Play(data));
        }

        private IEnumerator IE_Play(MusicData data)
        {
            yield return new WaitUntil(() => vanishFading <= 0);

            musicData = data;
            current = gameObject.GetOrAdd<AudioSource>();
            current.clip = musicData.clip;
            current.outputAudioMixerGroup = musicData.mixerGroup;
            current.loop = musicData.loop;
            current.volume = 0;
            current.bypassListenerEffects = true;
            current.Play();

            fading = 0.001f;
        }

        public void Stop()
        {
            if(current == null)
            {
                Debug.Log("Music current not running");
                return;
            }

            vanishFading = 0.001f;
        }

        private void Update()
        {
            HandleCrossFade();
            HandleCrossVanish();

            if (current && !current.isPlaying && playlist.Count > 0)
            {
                Debug.Log("Play next else");
                PlayNextTrack();
            }
        }

        private void HandleCrossFade()
        {
            if (fading <= 0f) 
                return;

            fading += Time.deltaTime;

            float fraction = Mathf.Clamp01(fading / crossFadeTime);
            // Logarithmic fade
            float logFraction = fraction.ToLogarithmicFraction();

            if (current) 
                current.volume = logFraction;

            if (fraction >= 1f)
            {
                fading = 0.0f;
            }
        }

        private void HandleCrossVanish()
        {
            if (vanishFading <= 0f)
                return;

            vanishFading += Time.deltaTime;

            float fraction = Mathf.Clamp01(vanishFading / crossFadeTime);
            // Logarithmic fade
            float logFraction = fraction.ToLogarithmicFraction();

            if (current)
                current.volume = 1.0f - logFraction;

            if (fraction >= 1f)
            {
                vanishFading = 0.0f;
            }
        }

        #region Setting
        [SerializeField]
        private AudioMixer audioMixer;
        private bool canMusic = true;
        public bool CanMusic
        {
            get
            {
                return canMusic;
            }

            set
            {
                canMusic = value;

                if(canMusic)
                {
                    if(!current.isPlaying)
                        Play(musicData);
                }
                else
                {
                    if(current.isPlaying)
                    {
                        current.Stop();
                    }
                        //Stop();
                }
            }
        }

        public void Volumne(float sliderVal)
        {
            float val = sliderVal.ToLogarithmicVolume();
            audioMixer.SetFloat("MusicVolume", val);
        }

        #endregion Setting
    }
}
