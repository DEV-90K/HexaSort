using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolParticle
{
    /// <summary>
    /// The Pool class represents the pool for a particular prefab.
    /// </summary>
    class Pool
    {
        Transform m_sRoot = null;

        //list prefab ready
        List<ParticleSystem> inactive;

        // The prefab that we are pooling
        ParticleSystem prefab;

        int index;

        // Constructor
        public Pool(ParticleSystem prefab, int initialQty, Transform parent)
        {
#if UNITY_EDITOR
            if (prefab.main.loop)
            {
                var main = prefab.main;
                main.loop = false;

                //save prefab
                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Not Loop");
                Debug.Log(prefab.name + " ~ Fix To Not Loop");
            }

            if (prefab.main.playOnAwake)
            {
                var main = prefab.main;
                main.playOnAwake = false;

                //save prefab
                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Not PlayAwake");
                Debug.Log(prefab.name + " ~ Fix To Not PlayAwake");
            }

            if (prefab.main.stopAction != ParticleSystemStopAction.None)
            {
                var main = prefab.main;
                main.stopAction = ParticleSystemStopAction.None;

                //save prefab
                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Stop Action None");
                Debug.Log(prefab.name + " ~ Fix To  Stop Action None");
            }

            if (prefab.main.duration > 1)
            {
                var main = prefab.main;
                main.duration = 1;

                //save prefab
                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Duration By 1");
                Debug.Log(prefab.name + " ~ Fix To Duration By 1");
            }
#endif

            m_sRoot = parent;
            this.prefab = prefab;
            inactive = new List<ParticleSystem>(initialQty);

            for (int i = 0; i < initialQty; i++)
            {
                ParticleSystem particle = (ParticleSystem)GameObject.Instantiate(prefab, m_sRoot);
                particle.Stop();
                inactive.Add(particle);
            }
        }

        public int Count
        {
            get { return inactive.Count; }
        }

        // Spawn an object from our pool
        public void Play(Vector3 pos, Quaternion rot)
        {
            index = index + 1 < inactive.Count ? index + 1 : 0;

            ParticleSystem obj = inactive[index];

            if (obj.isPlaying)
            {
                obj = (ParticleSystem)GameObject.Instantiate(prefab, m_sRoot);
                obj.Stop();
                inactive.Insert(index, obj);
            }

            obj.transform.SetPositionAndRotation(pos, rot);
            obj.Play();
        }

        public void Release()
        {
            while (inactive.Count > 0)
            {
                GameObject.DestroyImmediate(inactive[0]);
                inactive.RemoveAt(0);
            }
            inactive.Clear();
        }
    }
}
