using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform horse;         // Assign your horse
    public float rotationSmooth = 5f; // Smoothness for rotation
    public float positionSmooth = 5f; // Smoothness for position

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0f, 3f, -6f); // Default above/back

    private Vector3 currentVelocity;

    void LateUpdate()
    {
        if (horse == null) return;

        // Desired position relative to horse rotation
        Vector3 desiredPos = horse.position + horse.rotation * offset;

        // Smoothly move camera to position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref currentVelocity, 1f / positionSmooth);

        // Determine look direction: toward where the horse is moving
        Vector3 lookDirection = horse.forward;
        lookDirection.y = 0f; // keep camera level

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSmooth * Time.deltaTime);
        }
    }
}