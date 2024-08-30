using UnityEngine;

namespace PoolParticle
{
    public class PoolController : MonoBehaviour
    {
        [Header("---- POOL CONTROLER TO INIT POOL ----")]
        [Space]
        [Header("Particle")]
        public ParticleAmount[] Particle;

        private void Awake()
        {
            for (int i = 0; i < Particle.Length; i++)
            {
                ParticlePool.Preload(Particle[i].prefab, Particle[i].amount, Particle[i].root);
                ParticlePool.Shortcut(Particle[i].particleType, Particle[i].prefab);
            }
        }
    }
}
