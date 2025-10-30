using UnityEngine;

public class HorseBobbing : MonoBehaviour
{
    [Header("Bobbing Settings")]
    public float rotationAmount = 5f; // Max rotation in degrees
    public float bobSpeed = 2f;       // Speed of the bobbing

    private float initialXRotation;

    void Start()
    {
        // Store the initial X rotation to apply offsets
        initialXRotation = transform.localEulerAngles.x;
    }

    void Update()
    {
        // Mathf.PingPong gives a value that goes back and forth between 0 and rotationAmount*2
        float xRotationOffset = Mathf.PingPong(Time.time * bobSpeed, rotationAmount * 2) - rotationAmount;

        // Apply the rotation offset to the initial rotation
        Vector3 newRotation = transform.localEulerAngles;
        newRotation.x = initialXRotation + xRotationOffset;
        transform.localEulerAngles = newRotation;
    }
}
