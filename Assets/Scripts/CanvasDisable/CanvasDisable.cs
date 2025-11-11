using UnityEngine;

public class CanvasDisable : MonoBehaviour
{
    [Header("Assign the Canvas to disable")]
    public Canvas targetCanvas;

    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetCanvas != null)
            {
                targetCanvas.enabled = false;
                Debug.Log("Player exited - Canvas disabled.");
            }
        }
    }
}
