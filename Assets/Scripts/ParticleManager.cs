using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Header("Particle Prefabs")]
    public GameObject hitSparksEffect;
    public GameObject explosionEffect;
    public GameObject smokeEffect;
    public GameObject debrisEffect;

    public static ParticleManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayHitEffect(Vector3 position, Vector3 normal)
    {
        if (hitSparksEffect != null)
        {
            GameObject effect = Instantiate(hitSparksEffect, position, Quaternion.LookRotation(normal));
            Destroy(effect, 2f);
        }
    }

    public void PlayExplosionEffect(Vector3 position)
    {
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, position, Quaternion.identity);
            Destroy(effect, 3f);
        }
    }

    public void PlaySmokeEffect(Vector3 position)
    {
        if (smokeEffect != null)
        {
            GameObject effect = Instantiate(smokeEffect, position, Quaternion.identity);
            Destroy(effect, 5f);
        }
    }

    public void PlayDebrisEffect(Vector3 position)
    {
        if (debrisEffect != null)
        {
            GameObject effect = Instantiate(debrisEffect, position, Quaternion.identity);
            Destroy(effect, 4f);
        }
    }
}