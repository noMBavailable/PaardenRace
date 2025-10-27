using UnityEngine;
using UnityEngine.Splines;

public class SplineWallBuilder : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    [SerializeField] private GameObject colliderPrefab;
    [SerializeField] private int segmentCount = 30; // more = smoother curve
    [SerializeField] private float colliderLength = 1f;
    [SerializeField] private float colliderThickness = 0.2f;
    [SerializeField] private bool isTrigger = false;

    void Start()
    {
        if (spline == null || colliderPrefab == null)
        {
            Debug.LogError("Spline or Collider Prefab not assigned!");
            return;
        }

        for (int i = 0; i < segmentCount; i++)
        {
            float t1 = i / (float)segmentCount;
            float t2 = (i + 1) / (float)segmentCount;

            Vector3 pos1 = spline.EvaluatePosition(t1);
            Vector3 pos2 = spline.EvaluatePosition(t2);

            Vector3 mid = (pos1 + pos2) / 2f;
            Vector3 dir = (pos2 - pos1).normalized;
            float dist = Vector3.Distance(pos1, pos2);

            GameObject col = Instantiate(colliderPrefab, mid, Quaternion.LookRotation(dir), transform);
            col.name = $"SplineWall_{i}";
            col.transform.localScale = new Vector3(colliderThickness, 1f, dist / colliderLength);

            var box = col.GetComponent<BoxCollider>();
            if (box != null)
            {
                box.isTrigger = isTrigger;
            }
        }

        Debug.Log($"Generated {segmentCount} wall colliders along {name} spline.");
    }
}
