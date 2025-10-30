using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform horse; // Assign your horse here

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0f, 3f, -6f); // Adjustable position offset

    void LateUpdate()
    {
        if (horse == null) return;

        // Keep camera fixed relative to horse’s position and rotation
        transform.position = horse.position + horse.rotation * offset;

        // Make the camera look at the horse (slightly upward)
        transform.LookAt(horse.position + Vector3.up * 1.5f);// check if this is necessary
    }
}

