using UnityEngine;

// This script controls a third-person player with movement, jumping, crouching, camera look, and slope handling.
// It requires a CharacterController component.

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float crouchSpeed = 2f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;
    public float airControlPercent = 0.5f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Slope Handling")]
    public float slopeForce = 5f;
    public float slopeRayLength = 1.5f;

    [Header("Camera Look")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    public float pitchMin = -40f;
    public float pitchMax = 85f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float ySpeed;
    private float originalCenterY;

    private float pitch = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalCenterY = controller.center.y;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = controller.isGrounded;

        // Apply gravity manually
        if (isGrounded && ySpeed < 0)
        {
            ySpeed = -2f; // Slight downward force to stay grounded
        }

        // Handle camera rotation based on mouse movement
        HandleCameraLook();

        // Get movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        // Transform input direction relative to camera
        Vector3 moveDir = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * inputDir;

        // Determine movement speed
        float targetSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) targetSpeed = runSpeed;
        if (Input.GetKey(crouchKey)) targetSpeed = crouchSpeed;

        // Handle crouch toggle
        HandleCrouch();

        // If in air, reduce control
        float controlPercent = isGrounded ? 1f : airControlPercent;

        // Apply movement
        controller.Move(moveDir * targetSpeed * controlPercent * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded && !Input.GetKey(crouchKey))
        {
            ySpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        ySpeed += gravity * Time.deltaTime;
        velocity.y = ySpeed;
        controller.Move(velocity * Time.deltaTime);

        // Apply extra force when on slopes
        if (OnSlope() && isGrounded)
        {
            controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);
        }
    }

    // Handles camera rotation using mouse input
    void HandleCameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Tilt camera vertically
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        cameraTransform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }

    // Check if the character is standing on a slope
    bool OnSlope()
    {
        if (!isGrounded) return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeRayLength))
        {
            return hit.normal != Vector3.up;
        }
        return false;
    }

    // Handle crouch behavior
    void HandleCrouch()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            controller.height = crouchHeight;
            controller.center = new Vector3(0, crouchHeight / 2, 0);
        }
        else if (Input.GetKeyUp(crouchKey))
        {
            controller.height = standingHeight;
            controller.center = new Vector3(0, standingHeight / 2, 0);
        }
    }
}
