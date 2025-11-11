using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class LaneRacer : MonoBehaviour
{
    [System.Serializable]
    public class Checkpoint
    {
        public Transform[] lanes; // 0 = left, 1 = mid, 2 = right
    }

    [Header("Checkpoints & Lanes")]
    public List<Checkpoint> checkpoints;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float laneSwitchSpeed = 5f;
    public bool loopTrack = false; // Teleport back to start after last checkpoint
    public float checkpointProximity = 0.5f; // How close before advancing (XZ only)

    [Header("Debug Visualization")]
    public bool drawGizmos = true;
    public Color laneColor = Color.yellow;
    public Color checkpointColor = Color.cyan;
    public Color currentLaneColor = Color.green;

    private int currentCheckpointIndex = 0;
    private int currentLane = 1;
    private int targetLane = 1;
    private float forwardT = 0f;
    private float laneT = 0f;

    private void Update()
    {
        if (Application.isPlaying)
        {
            MoveForward();
            HandleLaneSwitchInput();
        }
    }

    void MoveForward()
    {
        if (checkpoints == null || checkpoints.Count < 2)
            return;

        int nextIndex = currentCheckpointIndex + 1;
        bool reachedEnd = nextIndex >= checkpoints.Count;

        // Handle teleport looping
        if (loopTrack && reachedEnd)
        {
            currentCheckpointIndex = 0;
            nextIndex = 1 % checkpoints.Count;
            forwardT = 0f;
        }
        else if (!loopTrack && reachedEnd)
        {
            return;
        }

        // Safe lane references
        Transform startCurrent = checkpoints[currentCheckpointIndex].lanes[currentLane];
        Transform endCurrent = checkpoints[nextIndex].lanes[currentLane];
        Transform startTarget = checkpoints[currentCheckpointIndex].lanes[targetLane];
        Transform endTarget = checkpoints[nextIndex].lanes[targetLane];

        if (!startCurrent || !endCurrent || !startTarget || !endTarget)
            return;

        // Calculate target positions
        Vector3 startPos = startCurrent.position;
        Vector3 endPos = endCurrent.position;

        // Update forward movement
        float segmentLength = Vector3.Distance(startPos, endPos);
        if (segmentLength < 0.001f) return;

        // Stop if within proximity (XZ distance)
        Vector3 flatPos = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 flatTarget = new Vector3(endPos.x, 0f, endPos.z);
        if (Vector3.Distance(flatPos, flatTarget) < checkpointProximity)
        {
            forwardT = 0f;
            currentCheckpointIndex = nextIndex;
            return;
        }

        forwardT += Time.deltaTime * moveSpeed / segmentLength;
        forwardT = Mathf.Clamp01(forwardT);

        // Smooth lane interpolation
        if (currentLane != targetLane)
        {
            laneT += Time.deltaTime * laneSwitchSpeed;
            if (laneT >= 1f)
            {
                laneT = 0f;
                currentLane = targetLane;
            }
        }

        // Interpolate forward along both lanes
        Vector3 currentLanePos = Vector3.Lerp(startCurrent.position, endCurrent.position, forwardT);
        Vector3 targetLanePos = Vector3.Lerp(startTarget.position, endTarget.position, forwardT);

        // Final blended position
        Vector3 targetPos = Vector3.Lerp(currentLanePos, targetLanePos, Mathf.SmoothStep(0, 1, laneT));

        // Keep current Y (don’t snap up/down if terrain is uneven)
        targetPos.y = transform.position.y;

        // Move
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // --- ROTATION ---
        // Rotate towards next point (ignore Y axis)
        Vector3 lookTarget = endCurrent.position;
        Vector3 direction = lookTarget - transform.position;
        direction.y = 0f; // ignore vertical tilt

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 2f);
        }
    }

    void HandleLaneSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            targetLane = Mathf.Max(0, targetLane - 1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            targetLane = Mathf.Min(2, targetLane + 1);
    }

    // --- GIZMOS SECTION ---
    private void OnDrawGizmos()
    {
        if (!drawGizmos || checkpoints == null)
            return;

        for (int i = 0; i < checkpoints.Count; i++)
        {
            var cp = checkpoints[i];
            if (cp == null || cp.lanes == null) continue;

            for (int l = 0; l < cp.lanes.Length; l++)
            {
                var lanePoint = cp.lanes[l];
                if (!lanePoint) continue;

                // Draw checkpoint spheres
                Gizmos.color = (l == currentLane) ? currentLaneColor : checkpointColor;
                Gizmos.DrawSphere(lanePoint.position, 0.15f);

                // Connect lanes within the same checkpoint
                if (l < cp.lanes.Length - 1 && cp.lanes[l + 1] != null)
                {
                    Gizmos.color = Color.Lerp(Color.red, Color.blue, l / 2f);
                    Gizmos.DrawLine(cp.lanes[l].position, cp.lanes[l + 1].position);
                }

                // Connect lane paths between checkpoints
                if (i < checkpoints.Count - 1)
                {
                    if (checkpoints[i + 1] != null && checkpoints[i + 1].lanes.Length > l && checkpoints[i + 1].lanes[l] != null)
                    {
                        Gizmos.color = laneColor;
                        Gizmos.DrawLine(cp.lanes[l].position, checkpoints[i + 1].lanes[l].position);
                    }
                }
                // Transparent loop connection (last → first)
                else if (loopTrack && checkpoints.Count > 1)
                {
                    var firstCp = checkpoints[0];
                    if (firstCp != null && firstCp.lanes.Length > l && firstCp.lanes[l] != null)
                    {
                        Color semiTransparent = laneColor;
                        semiTransparent.a = 0.25f;
                        Gizmos.color = semiTransparent;
                        Gizmos.DrawLine(cp.lanes[l].position, firstCp.lanes[l].position);
                    }
                }
            }
        }
    }
}
