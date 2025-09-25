using UnityEngine;

public class ThirdPersonCameraFollow : MonoBehaviour
{
    public Transform target; // Player
    public float distance = 4f; // Distance from the target
    public float height = 2f; // Height above the target
    public float rotationSpeed = 5f; // Mouse sensitivity
    public float followSpeed = 10f; // Smoothing

    private float currentYaw = 0f;

    void LateUpdate()
    {
        if (target == null) return;

        // Get horizontal mouse movement
        float mouseX = Input.GetAxis("Mouse X");

        // Rotate around the Y-axis based on mouse movement
        currentYaw += mouseX * rotationSpeed;

        // Calculate desired position
        Quaternion rotation = Quaternion.Euler(0f, currentYaw, 0f);
        Vector3 offset = rotation * new Vector3(0, height, -distance);
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Always look at the target (lock camera tilt to horizontal plane)
        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0f; // Lock the X-axis rotation
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }
}
