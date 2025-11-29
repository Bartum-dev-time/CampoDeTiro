using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Target Settings")]
    public int maxHealth = 100;
    public int scoreValue = 10;
    public bool respawnAfterDestroy = true;
    public float respawnTime = 3f;

    [Header("Visual Effects")]
    public GameObject destroyEffectPrefab;
    public Material hitMaterial;
    public float hitFlashDuration = 0.1f;

    [Header("Animation")]
    public bool useAnimation = true;
    public string hitAnimationTrigger = "Hit";
    public string destroyAnimationTrigger = "Destroy";

    private int currentHealth;
    private Animator animator;
    private Renderer targetRenderer;
    private Material originalMaterial;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        targetRenderer = GetComponent<Renderer>();

        if (targetRenderer != null)
            originalMaterial = targetRenderer.material;

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Efecto visual de golpe
        if (hitMaterial != null && targetRenderer != null)
        {
            StartCoroutine(HitFlash());
        }

        // Animación de golpe
        if (useAnimation && animator != null)
        {
            animator.SetTrigger(hitAnimationTrigger);
        }

        // Verificar si fue destruido
        if (currentHealth <= 0)
        {
            DestroyTarget();
        }
    }

    System.Collections.IEnumerator HitFlash()
    {
        targetRenderer.material = hitMaterial;
        yield return new WaitForSeconds(hitFlashDuration);
        targetRenderer.material = originalMaterial;
    }

    void DestroyTarget()
    {
        // Notificar al GameManager
        GameManager gameManager = Object.FindAnyObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.AddScore(scoreValue);
            gameManager.TargetDestroyed();
        }

        // Animación de destrucción
        if (useAnimation && animator != null)
        {
            animator.SetTrigger(destroyAnimationTrigger);
        }

        // Efecto de partículas
        if (destroyEffectPrefab != null)
        {
            GameObject effect = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 3f);
        }

        // Respawn o destruir
        if (respawnAfterDestroy)
        {
            StartCoroutine(RespawnTarget());
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }
    }

    System.Collections.IEnumerator RespawnTarget()
    {
        // Ocultar temporalmente
        GetComponent<Collider>().enabled = false;
        if (targetRenderer != null)
            targetRenderer.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        // Restablecer
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        currentHealth = maxHealth;

        // Mostrar nuevamente
        GetComponent<Collider>().enabled = true;
        if (targetRenderer != null)
            targetRenderer.enabled = true;
    }

    // Target móvil opcional
    [Header("Moving Target (Optional)")]
    public bool isMoving = false;
    public float moveSpeed = 2f;
    public float moveRange = 5f;

    private float moveDirection = 1f;

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.right * moveSpeed * moveDirection * Time.deltaTime);

            if (Mathf.Abs(transform.position.x - originalPosition.x) > moveRange)
            {
                moveDirection *= -1;
            }
        }
    }
}