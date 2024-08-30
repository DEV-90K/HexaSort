using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolParticle
{
    [System.Serializable]
    public class ParticleAmount
    {
        public Transform root;
        public ParticleType particleType;
        public ParticleSystem prefab;
        public int amount;
    }


    public enum ParticleType
    {
        MagicBuffBlue,
        MagicAuraBlue
    }
}
