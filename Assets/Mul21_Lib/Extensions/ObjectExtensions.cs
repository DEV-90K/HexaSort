using UnityEngine;

namespace UnityUtils
{
    public static class ObjectExtensions
    {
        public static T CopyObject<T>(this T obj)
        {
            string json = JsonUtility.ToJson(obj);
            T newObject = JsonUtility.FromJson<T>(json);
            return newObject;
        }  
        
        public static void DebugLogObject<T>(this T obj)
        {
            string json = JsonUtility.ToJson(obj);
            Debug.Log(typeof(T) + " " + json);
        }
    }
}
