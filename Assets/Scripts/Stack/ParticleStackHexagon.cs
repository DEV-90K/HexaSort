using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleStackHexagon : MonoBehaviour
{
    [SerializeField]
    private GameObject _AoEHolySword;
    [SerializeField]
    private GameObject _AoEIceStorm;
    [SerializeField]
    private GameObject _AoEStars;

    [SerializeField]
    private MeshCollider _MeshCollider;

    private StackHexagon _stackHexagon;
    private GameObject _particle;

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
                _particle = Instantiate(_AoEHolySword, transform);
                break;
            case VanishType.AROUND:
                _particle = Instantiate(_AoEIceStorm, transform);
                break;
            case VanishType.CONTAIN_COLOR:
                _particle = Instantiate(_AoEStars, transform);
                break;
        }

        _MeshCollider.enabled = true;
        yield return WaitUntilCompleted();
        _MeshCollider.enabled = false;

        DestroyImmediate(_particle);
        OnHide();
    }

    public IEnumerator WaitUntilCompleted()
    {
        yield return new WaitForSeconds(1f);
    }
}
