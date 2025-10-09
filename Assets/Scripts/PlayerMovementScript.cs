using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class ThirdPersonMovement : MonoBehaviour
{
    // --- Components & References ---
    private Rigidbody rb;
    public Transform cameraTransform; // Assign the Main Camera's Transform here

    // --- Movement Variables ---
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 2f;
    public float rotationSpeed = 500f; // For smooth rotation to movement direction
    private float currentSpeed;

    // --- Jumping & Ground Check ---
    public float jumpForce = 8f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
    private bool isGrounded;

    // --- Crouching Variables ---
    private bool isCrouching = false;
    private float defaultHeight; // Store the original collider height
    public float crouchHeight = 1.0f;
    private CapsuleCollider capCollider;

    // --- Input Variables ---
    private Vector3 moveDirection;

    // -----------------------------------------------------------------

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capCollider = GetComponent<CapsuleCollider>();
        // Freeze rotation to let the script handle it for smooth control
        rb.freezeRotation = true;

        // Store the default height for toggling crouch
        defaultHeight = capCollider.height;
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        // 1. Handle Input (PC Controls)
        HandleInput();

        // 2. Handle State Transitions
        HandleCrouching();
        HandleSprinting();

        // 3. Jump Input
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        // Physics updates (Ground Check and Movement Application)
        CheckGroundStatus();
        MovePlayer();
    }

    // -----------------------------------------------------------------

    /// <summary>
    /// Reads WASD input and converts it into a world-space direction 
    /// relative to the camera's orientation.
    /// </summary>
    private void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float v = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // Calculate the direction in camera-space
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        // Transform the camera-space direction to world-space
        if (inputDir.magnitude >= 0.1f)
        {
            // Get the camera's forward and right vectors, ignoring Y-axis for 2D plane movement
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // Calculate the final move direction
            moveDirection = (camRight * h + camForward * v).normalized;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    /// <summary>
    /// Applies the calculated move direction to the Rigidbody.
    /// Also handles rotation to face the direction of movement.
    /// </summary>
    private void MovePlayer()
    {
        Vector3 targetVelocity = moveDirection * currentSpeed;

        // Set the horizontal velocity, keeping the existing vertical (Y) velocity
        rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);

        // Handle Player Rotation (only when moving)
        if (moveDirection.magnitude >= 0.1f)
        {
            // Calculate the rotation needed to face the move direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // Smoothly interpolate current rotation towards the target rotation
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }

    /// <summary>
    /// Checks if the player is touching the ground using a raycast.
    /// </summary>
    private void CheckGroundStatus()
    {
        // Raycast from the bottom of the collider downwards
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            capCollider.height / 2 + groundCheckDistance,
            groundLayer
        );
    }

    /// <summary>
    /// Applies an upward force to the Rigidbody for jumping.
    /// </summary>
    private void Jump()
    {
        // Clear existing vertical velocity for consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Handles the speed change when holding the left shift key.
    /// </summary>
    private void HandleSprinting()
    {
        // If the player is trying to sprint, is moving, and is not crouching
        if (Input.GetKey(KeyCode.LeftShift) && moveDirection.magnitude >= 0.1f && !isCrouching)
        {
            currentSpeed = sprintSpeed;
        }
        else if (isCrouching)
        {
            // Speed is already set to crouchSpeed in HandleCrouching
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

    /// <summary>
    /// Toggles the crouching state and adjusts the collider height/speed.
    /// </summary>
    private void HandleCrouching()
    {
        if (Input.GetKeyDown(KeyCode.C)) // Use 'C' to toggle crouch
        {
            isCrouching = !isCrouching;

            if (isCrouching)
            {
                // Enter Crouch state
                capCollider.height = crouchHeight;
                currentSpeed = crouchSpeed;
                // Optional: Adjust center to keep bottom of collider in place
                capCollider.center = new Vector3(0, crouchHeight / 2, 0);
            }
            else
            {
                // Exit Crouch state
                // **Add a check here (Raycast) to ensure player can stand up (no ceiling)**
                capCollider.height = defaultHeight;
                currentSpeed = walkSpeed;
                capCollider.center = new Vector3(0, defaultHeight / 2, 0);
            }
        }
    }
}