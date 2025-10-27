using UnityEngine;
using UnityEngine.Splines;

public class SplineTriggerBuilder : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    [SerializeField] private GameObject colliderPrefab;
    [SerializeField] private int segmentCount = 20;

    void Start()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            float t1 = i / (float)segmentCount;
            float t2 = (i + 1) / (float)segmentCount;

            Vector3 p1 = spline.EvaluatePosition(t1);
            Vector3 p2 = spline.EvaluatePosition(t2);
            Vector3 mid = (p1 + p2) / 2f;
            Quaternion rot = Quaternion.LookRotation(p2 - p1);
            float length = Vector3.Distance(p1, p2);

            GameObject col = Instantiate(colliderPrefab, mid, rot, transform);
            col.transform.localScale = new Vector3(0.2f, 2f, length);
        }

        Debug.Log("✅ Colliders spawned along spline!");
    }
}
