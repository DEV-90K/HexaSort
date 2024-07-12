using System.Collections;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private ParticleSystem Particle;

    public IEnumerator IE_HummerAction()
    {
        Animator.Play("Hammer-A", 0);
        yield return new WaitForSeconds(Animator.GetCurrentAnimatorClipInfo(0).Length);
        Particle.Play();
    } 
}
