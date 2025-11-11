// using UnityEngine;

// public class HorseBobbing : MonoBehaviour
// {
//     [Header("Bobbing Settings")]
//     public float rotationAmount = 5f; // Max rotation in degrees
//     public float bobSpeed = 2f;       // Speed of the bobbing

//     private float initialXRotation;

//     void Start()
//     {
//         // Store the initial X rotation to apply offsets
//         initialXRotation = transform.localEulerAngles.x;
//     }

//     void Update()
//     {
//         // Mathf.PingPong gives a value that goes back and forth between 0 and rotationAmount*2
//         float xRotationOffset = Mathf.PingPong(Time.time * bobSpeed, rotationAmount * 2) - rotationAmount;

//         // Apply the rotation offset to the initial rotation
//         Vector3 newRotation = transform.localEulerAngles;
//         newRotation.x = initialXRotation + xRotationOffset;
//         transform.localEulerAngles = newRotation;
//     }
// }
using UnityEngine;

public class HorseBobbing : MonoBehaviour
{
    [Header("References")]
    public GameObject model;

    [Header("Bobbing Settings")]
    public float rotationAmount = 5f;
    public float bobSpeed = 2f;
    public float verticalAmount = 0.05f; // How high/low the model moves

    private float initialXRotation;
    private Vector3 initialLocalPosition;

    void Start()
    {
        if (model == null)
        {
            Debug.LogWarning($"{nameof(HorseBobbing)}: No model assigned. Using this GameObject instead.");
            model = gameObject;
        }

        initialXRotation = model.transform.localEulerAngles.x;
        initialLocalPosition = model.transform.localPosition;
    }

    void Update()
    {
        float time = Time.time * bobSpeed;

        // Gentle up/down motion using sine
        float yOffset = Mathf.Sin(time) * verticalAmount * 2f;

        // Gentle pitch forward/back motion using sine (offset to match rhythm)
        float xRotationOffset = Mathf.Sin(time * 2f) * rotationAmount;

        // Apply new rotation and position
        model.transform.localEulerAngles = new Vector3(initialXRotation + xRotationOffset, 0f, 0f);
        model.transform.localPosition = initialLocalPosition + new Vector3(0f, yOffset, 0f);
    }
}