using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleStackHexagon : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] _Particles;
    [SerializeField]
    private GameObject _AoEHolySword;
    [SerializeField]
    private GameObject _AoEIceStorm;
    [SerializeField]
    private GameObject _AoEStars;
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

        for(int i = 3; i < transform.childCount; i++)
        {
            Object.DestroyImmediate(transform.GetChild(i));
        }
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
                //_particle = _Particles[0];
                _particle = Instantiate(_AoEHolySword, transform).GetComponent<ParticleSystem>();
                break;
            case VanishType.AROUND:
                //_particle = _Particles[1];
                _particle = Instantiate(_AoEIceStorm, transform).GetComponent<ParticleSystem>();
                break;
            case VanishType.CONTAIN_COLOR:
                //_particle = _Particles[2];
                _particle = Instantiate(_AoEStars, transform).GetComponent<ParticleSystem>();
                break;
        }

        _particle.gameObject.SetActive(true);
        _particle.playbackSpeed= 5f;
        _particle.Play();
        _MeshCollider.enabled = true;
        yield return WaitUntilCompleted();
        _MeshCollider.enabled = false;
        _particle.Stop();
        DestroyImmediate(_particle);
        OnHide();
    }

    public IEnumerator WaitUntilCompleted()
    {
        yield return new WaitUntil(() => _particle.isPlaying == false);
    }
}
