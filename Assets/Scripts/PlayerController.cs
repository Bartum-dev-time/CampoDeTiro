using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float verticalLookLimit = 80f;

    [Header("References")]
    public Transform cameraTransform;

    private CharacterController controller;
    private float verticalRotation = 0f;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * horizontal + transform.forward * vertical;

        // Aplicar movimiento
        controller.Move(direction * moveSpeed * Time.deltaTime);

        // Animación (si tienes animator)
        if (animator != null)
        {
            float speed = direction.magnitude;
            animator.SetFloat("Speed", speed);
        }

        // Gravedad simple
        controller.Move(Vector3.down * 9.81f * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        // Rotación horizontal (personaje)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        // Rotación vertical (cámara)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}