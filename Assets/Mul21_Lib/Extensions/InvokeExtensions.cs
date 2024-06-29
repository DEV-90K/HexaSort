using System.Collections;
using UnityEngine;

namespace Mul21_Lib
{
    //USING
    //this.Invoke(MyFunctionNoParams, 1f);
    //this.Invoke(() => MyFunctionParams(0, false), 1f);
    //this.Invoke(()=>Debug.Log("Lambdas also work"), 1f);
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
}
