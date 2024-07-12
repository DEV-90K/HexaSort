using UnityEngine;

//Remove Old Instance and init base InitializationTime
public class RealtimeMonoSingleton<T> : MonoSingleton<T> where T : Component
{
    public float InitializationTime { get; private set; }

    /// <summary>
    /// Make sure to call base.Awake() in override if you need awake.
    /// </summary>

    protected override void InitializeSingleton()
    {
        if (!Application.isPlaying) return;
        InitializationTime = Time.time;
        DontDestroyOnLoad(gameObject);

        T[] oldInstances = FindObjectsByType<T>(FindObjectsSortMode.None);
        foreach (T old in oldInstances)
        {
            if (old.GetComponent<RealtimeMonoSingleton<T>>().InitializationTime < InitializationTime)
            {
                Destroy(old.gameObject);
            }
        }

        if (instance == null)
        {
            instance = this as T;
        }
    }
}
