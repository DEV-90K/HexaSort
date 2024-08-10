using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorExtensions
{
    // Wait for current the transition to end
    public static IEnumerator IE_WaitTransition(this Animator anim)
    {
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
    }

    // Wait for current the animation to end
    public static IEnumerator IE_WaitAnimation(this Animator anim)
    {
        yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
    }

    // Wait for the animation to end
    public static IEnumerator IE_WaitAnimation(this Animator anim, string animState)
    {
        yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).IsName(animState));
    }
}
