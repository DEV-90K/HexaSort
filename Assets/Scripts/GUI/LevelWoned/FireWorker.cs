using System.Collections;
using UnityEngine;

namespace GUILevelWoned
{
    public class FireWorker : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _ParticleSystem;
        private Coroutine _Process = null;

        public void PlayOnce()
        {
            ParticleSystem.MainModule mainModule = _ParticleSystem.main;
            mainModule.loop = false;
            PlayParticle();
            _Process = StartCoroutine(IE_ParticlePlaying());
        }

        public void PlayRepeat(int amount)
        {
            StartCoroutine(IE_PlayRepeat(amount));
        }

        public IEnumerator IE_PlayRepeat(int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                PlayOnce();
                yield return new WaitUntil(() => CheckPlayCompleted());
            }
        }

        private bool CheckPlayCompleted()
        {
            return _Process == null;
        }

        public void PlayForever()
        {
            ParticleSystem.MainModule mainModule = _ParticleSystem.main;
            mainModule.loop = true;
            PlayParticle();
            _Process = StartCoroutine(IE_ParticlePlaying());
        }

        private IEnumerator IE_ParticlePlaying()
        {
            yield return new WaitUntil(() => _ParticleSystem.isPlaying == false);
            _Process = null;
        }

        private void PlayParticle()
        {
            _ParticleSystem.Play();
        }

        public void StopParticle()
        {
            if (_Process != null)
                StopCoroutine(_Process);

            _Process = null;
            _ParticleSystem.Stop();            
        }
    }
}
