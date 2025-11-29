using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShootingSystem : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float shootRange = 100f;
    public float fireRate = 0.5f;
    public int damage = 50;
    public LayerMask targetLayer;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public GameObject hitEffectPrefab;
    public GameObject bulletTrail;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip hitSound;

    [Header("References")]
    public Transform firePoint;
    public Camera fpsCam;
    public Animator weaponAnimator;

    private float nextFireTime = 0f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (fpsCam == null)
            fpsCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Animación de disparo
        if (weaponAnimator != null)
            weaponAnimator.SetTrigger("Shoot");

        // Efecto de partículas de disparo
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Sonido de disparo
        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound);

        // Raycast para detectar impacto
        RaycastHit hit;
        Vector3 shootDirection = fpsCam.transform.forward;

        if (Physics.Raycast(fpsCam.transform.position, shootDirection, out hit, shootRange, targetLayer))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // Verificar si golpeó un objetivo
            Target target = hit.collider.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            // Efecto visual de impacto
            if (hitEffectPrefab != null)
            {
                GameObject hitEffect = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitEffect, 2f);
            }

            // Sonido de impacto
            if (audioSource != null && hitSound != null)
                audioSource.PlayOneShot(hitSound);

            // Trail de bala opcional
            if (bulletTrail != null)
            {
                StartCoroutine(CreateBulletTrail(firePoint.position, hit.point));
            }
        }
        else
        {
            // Disparo sin impacto
            if (bulletTrail != null)
            {
                StartCoroutine(CreateBulletTrail(firePoint.position, firePoint.position + shootDirection * shootRange));
            }
        }
    }

    System.Collections.IEnumerator CreateBulletTrail(Vector3 start, Vector3 end)
    {
        GameObject trail = Instantiate(bulletTrail, start, Quaternion.identity);
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null)
        {
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }

        yield return new WaitForSeconds(0.1f);
        Destroy(trail);
    }

    // Visualización en editor
    void OnDrawGizmosSelected()
    {
        if (fpsCam != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * shootRange);
        }
    }
}