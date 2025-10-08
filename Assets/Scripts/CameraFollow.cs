using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera")]
    public Transform target;
    public float distance = 5.0f;
    public float height = 2.0f;
    public float smoothSpeed = 0.125f;

    private Vector3 offset;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate the desired position of the camera
        Vector3 desiredPosition = target.position - transform.forward * distance;
        desiredPosition.y = target.position.y + height;

        // Smoothly move the camera to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Make the camera look at the target
        transform.LookAt(target);
    }
}