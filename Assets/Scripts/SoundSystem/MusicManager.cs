
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio_System
{
    public class MusicManager : PersistentMonoSingleton<MusicManager>
    {
        [SerializeField] List<MusicData> initialMusicList;

        private const float crossFadeTime = 1.0f;
        private float fading = 0f;
        private float vanishFading = 0f;

        private MusicData musicData = null;

        private AudioSource previous = null;
        private AudioSource current = null;

        private readonly Queue<MusicData> playlist = new();
        Coroutine coroutinePlay = null;

        [SerializeField] private AudioMixer mixer = null;

        void Start()
        {
            //foreach (MusicData data in initialMusicList)
            //{
            //    AddToPlaylist(data);
            //}

            mixer.SetFloat("MusicVolume", Mathf.Log10(0.001f) * 20f);
        }

        public void AddToPlaylist(MusicData data)
        {
            playlist.Enqueue(data);
            if (current == null && previous == null)
            {
                PlayNextTrack();
            }
        }

        public void Clear()
        {
            playlist.Clear();

            musicData = null;
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
            if (current && current.clip == data.clip)
            {
                return;
            }

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

        public void Stop(MusicData data)
        {
            if(current == null || current.clip != data.clip)
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

            Debug.Log("fraction: " + fraction);
            if (fraction >= 1f)
            {
                vanishFading = 0.0f;
            }
        }
    }
}
