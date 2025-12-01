using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ParticleSystem muzzleFashParticles;
    public ParticleSystem bulleHitParticles;
    public ParticleSystem explosionParticle;
    public TrailRenderer bulletTracer;
    public Transform firePoint;
    Ray ray;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FireGun();
        }
    }

    void FireGun()
    {
        if (muzzleFashParticles != null)
        {
            muzzleFashParticles.transform.position = firePoint.position;
            muzzleFashParticles.Emit(1);
        }
        var tracer = Instantiate(bulletTracer, ray.origin, Quaternion.identity);
        tracer.AddPosition(firePoint.position);

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit))
        {

            if (bulleHitParticles != null)
            {
                bulleHitParticles.transform.position = hit.point;
                bulleHitParticles.transform.forward = hit.normal;

                bulleHitParticles.Play();

                tracer.transform.position = hit.point;
            }

            //if (hit.collider.CompareTag("CanExplote"))
            //{
            //    explosionParticle.transform.position = hit.point;
            //    explosionParticle.Play();
            //    Destroy(hit.collider.gameObject);
            //}
        }
    }
}
