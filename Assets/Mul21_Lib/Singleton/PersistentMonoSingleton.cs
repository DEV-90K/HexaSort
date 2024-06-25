using Mul21_Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mul21_Lib
{
    public class PersistentMonoSingleton<T> : MonoSingleton<T> where T : Component
    {
        public bool AutoUnparentOnAwake = true;

        /// <summary>
        /// Make sure to call base.Awake() in override if you need awake.
        /// </summary>

        protected override void InitializeSingleton()
        {
            if (!Application.isPlaying) 
                return;

            if (AutoUnparentOnAwake)
            {
                transform.SetParent(null);
            }

            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
