using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem.CollisionModule _particleCollision;

    private void Awake()
    {
        LayerMask layer = 1 << gameObject.layer;
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();

        _particleCollision = particleSystem.collision;
        _particleCollision.collidesWith = layer;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.GetInstanceID() == gameObject.GetInstanceID())
        {
            Debug.Log("Contact the same");
        }
        else
        {
            Debug.Log("Contact with: " + other.name);
            Debug.Log(other.GetInstanceID());
        }
    }
}
