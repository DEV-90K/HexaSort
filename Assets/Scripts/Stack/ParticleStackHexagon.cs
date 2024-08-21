using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleStackHexagon : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] _Particles;
    [SerializeField]
    private MeshCollider _MeshCollider;

    private StackHexagon _stackHexagon;
    private ParticleSystem _particle;

    private void Start()
    {
        _MeshCollider.enabled = false;
    }

    public void OnInit(StackHexagon stack)
    {
        _stackHexagon = stack;
        _MeshCollider.enabled = false;
    }

    private void OnShow()
    {
        gameObject.SetActive(true);
        transform.position = _stackHexagon.GetTopPosition();
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator PlayParticle(VanishType type)
    {
        OnShow();
        switch (type)
        {
            case VanishType.RANDOM:
                _particle = _Particles[0];
                break;
            case VanishType.AROUND:
                _particle = _Particles[1];
                break;
            case VanishType.CONTAIN_COLOR:
                _particle = _Particles[2];
                break;
        }

        _particle.gameObject.SetActive(true);
        _particle.playbackSpeed= 5f;
        _particle.Stop();
        _particle.Play();
        _MeshCollider.enabled = true;
        yield return WaitUntilCompleted();
        _MeshCollider.enabled = false;
        _particle.Stop();
        _particle.gameObject.SetActive(false);
        OnHide();
    }

    public IEnumerator WaitUntilCompleted()
    {
        yield return new WaitUntil(() => _particle.isPlaying == false);
    }
}
