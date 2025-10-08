using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Movement configuration
    [Header("Movement")]
    public float walkSpeed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float turnSmoothTime = 0.1f;

    // Jump and gravity configuration
    [Header("Jumping & Gravity")]
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;

    // Components
    private CharacterController controller;
    private Transform mainCamera;

    // Internal state variables
    private float turnSmoothVelocity;
    private Vector3 velocity;

    void Start()
    {
        // Get references to necessary components
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        HandleMovement();
        HandleJumpingAndGravity();
    }

    private void HandleMovementAndRotation()
    {
        // ⌨️ Input from keyboard
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Create a direction vector based on input (local space X, Z)
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            // 1. Get camera's forward direction but IGNORE pitch (Y-axis)
            Vector3 cameraForward = mainCameraTransform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            // 2. Create a rotation that aligns the world-forward with the camera's horizontal forward
            Quaternion cameraRotation = Quaternion.LookRotation(cameraForward, Vector3.up);

            // 3. Convert the WASD input (inputDirection) into a world-space vector relative to the camera
            Vector3 finalMoveDirection = cameraRotation * inputDirection;

            // 4. Calculate the target angle for the player's rotation
            float targetRotationAngle = Mathf.Atan2(finalMoveDirection.x, finalMoveDirection.z) * Mathf.Rad2Deg;

            // 5. Smoothly apply the rotation to the player
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotationAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // 6. Determine speed and apply movement
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

            // Move the character using the calculated world-space direction
            controller.Move(finalMoveDirection.normalized * currentSpeed * Time.deltaTime);
        }
    }

    private void HandleJumpingAndGravity()
    {
        // If the character is on the ground, and their vertical velocity is negative, reset it
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Check for jump input
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            // Apply a vertical velocity for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity to the character's vertical velocity every frame
        velocity.y += gravity * Time.deltaTime;

        // Apply the final vertical movement
        controller.Move(velocity * Time.deltaTime);
    }
}