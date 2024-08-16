using System;
using System.Reflection;
using UnityEngine;

public static class GameObjectExtensions
{
    public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (!component) component = gameObject.AddComponent<T>();

        return component;
    }

    public static T CopyComponent<T>(this GameObject obj, T component) where T : Component
    {
        Type type = component.GetType();
        Component copy = obj.GetComponent<T>();
        if(!copy)
            obj.AddComponent<T>();

        FieldInfo[] fields = type.GetFields();

        foreach (FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(component));
        }

        return copy as T;
    }    
}
