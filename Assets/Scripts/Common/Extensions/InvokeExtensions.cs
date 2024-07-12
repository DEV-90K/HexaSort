using System.Collections;
using UnityEngine;

public static class InvokeExtensions
{
    public static void Invoke(this MonoBehaviour monoInvoke, System.Action callback, float delay)
    {
        monoInvoke.StartCoroutine(InvokeRoutine(callback, delay));
    }

    private static IEnumerator InvokeRoutine(System.Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }
}
