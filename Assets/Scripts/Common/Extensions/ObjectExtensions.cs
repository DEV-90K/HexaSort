using Newtonsoft.Json;
using UnityEngine;

public static class ObjectExtensions
{
    public static T CopyObject<T>(this T obj)
    {
        string json = JsonConvert.SerializeObject(obj);
        T newObject = JsonConvert.DeserializeObject<T>(json);
        return newObject;
    }

    public static void DebugLogObject<T>(this T obj)
    {
        string json = JsonConvert.SerializeObject(obj);
        Debug.Log(typeof(T) + " " + json);
    }

    public static bool CompareObject(this GameObject obj, GameObject objCompare)
    {
        return GameObject.ReferenceEquals(obj, objCompare);
    }
}
