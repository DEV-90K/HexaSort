using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mul21_Lib
{
    //Awake Singleton
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        protected static T instance;

        protected static bool HasInstance => instance != null;
        protected static T TryGetInstance() => HasInstance ? instance : null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<T>();
                    if (instance == null)
                    {
                        var go = new GameObject(typeof(T).Name + " Auto-Generated");
                        instance = go.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying) 
                return;

            instance = this as T;
        }
    }
}
