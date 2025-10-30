using UnityEngine;

public class SplineTriggerPart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Horse")) // or "Player" if you tagged it
        {
            Debug.Log($"{name} hit by {other.name}");
        }
    }

    // Optional: visualize the trigger in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        BoxCollider col = GetComponent<BoxCollider>();
        if (col != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(col.center, col.size);
        }
    }
}
