using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolParticle
{
    public static class ParticlePool
    {
        const int DEFAULT_POOL_SIZE = 3;

        private static Transform root;

        public static Transform Root
        {
            get
            {
                if (root == null)
                {
                    PoolController controler = GameObject.FindObjectOfType<PoolParticle.PoolController>();
                    root = controler != null ? controler.transform : new GameObject("ParticlePool").transform;
                }

                return root;
            }
        }

        //--------------------------------------------------------------------------------------------------

        static Dictionary<ParticleType, ParticleSystem> shortcuts = new Dictionary<ParticleType, ParticleSystem>();
        // All of our pools
        static Dictionary<int, Pool> pools = new Dictionary<int, Pool>();

        /// <summary>
        /// Init our dictionary.
        /// </summary>
        static void Init(ParticleSystem prefab = null, int qty = DEFAULT_POOL_SIZE, Transform parent = null)
        {
            if (prefab != null && !pools.ContainsKey(prefab.GetInstanceID()))
            {
                pools[prefab.GetInstanceID()] = new Pool(prefab, qty, parent);
            }
        }

        static public void Preload(ParticleSystem prefab, int qty = 1, Transform parent = null)
        {
            Init(prefab, qty, parent);
        }

        static public void Play(ParticleSystem prefab, Vector3 pos, Quaternion rot)
        {
#if UNITY_EDITOR
            if (prefab == null)
            {
                Debug.LogError(prefab.name + " is null!");
                return;
            }
#endif

            if (!pools.ContainsKey(prefab.GetInstanceID()))
            {
                Transform newRoot = new GameObject("VFX_" + prefab.name).transform;
                newRoot.SetParent(Root);
                pools[prefab.GetInstanceID()] = new Pool(prefab, 10, newRoot);
            }

            pools[prefab.GetInstanceID()].Play(pos, rot);
        }

        static public void Play(ParticleType particleType, Vector3 pos, Quaternion rot)
        {
#if UNITY_EDITOR
            if (!shortcuts.ContainsKey(particleType))
            {
                Debug.LogError(particleType + " is nees install at pool container!!!");
            }
#endif

            Play(shortcuts[particleType], pos, rot);
        }

        static public void Release(ParticleSystem prefab)
        {
            if (pools.ContainsKey(prefab.GetInstanceID()))
            {
                pools[prefab.GetInstanceID()].Release();
                pools.Remove(prefab.GetInstanceID());
            }
            else
            {
                GameObject.DestroyImmediate(prefab);
            }
        }

        static public void Release(ParticleType particleType)
        {
#if UNITY_EDITOR
            if (!shortcuts.ContainsKey(particleType))
            {
                Debug.LogError(particleType + " is nees install at pool container!!!");
            }
#endif

            Release(shortcuts[particleType]);
        }

        static public void Shortcut(ParticleType particleType, ParticleSystem particleSystem)
        {
            shortcuts.Add(particleType, particleSystem);
        }
    }
}
