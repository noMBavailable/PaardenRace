using UnityEngine;
using UnityEngine.Splines;
using System.Collections; 

public class Movement : MonoBehaviour
{
    [Header("Lane movement")]
    [SerializeField] private float moveAmount = 1.0f;          // Distance between lanes
    [SerializeField] private float moveSpeedSideways = 5.0f;  // How fast to slide
    private int positionIndex = 1;                             // Current lane (0 = left, 1 = middle, 2 = right)
    private float laneOffset = 0f;                             // X offset from spline
    [SerializeField] private SplineAnimate splineAnimate;
    private ActivateSpline activateSpline;
    [SerializeField] private SplineAnimate[] splines;


    private bool isSlowingDown = false;

    // Adjustable forward factor for diagonal movement
    [SerializeField] private float forwardOffsetFactor = 0.5f;

    void Start()
    {
        activateSpline = FindObjectOfType<ActivateSpline>();
    }

    void Update()
    {
        HandleLaneInput();

        // Calculate lane offset
        laneOffset = (positionIndex - 1) * moveAmount;

        // Move relative to spline with diagonal motion
        if (splineAnimate != null)
        {
            Vector3 splinePosition = splineAnimate.transform.position;

            // Get spline local directions
            Vector3 splineForward = splineAnimate.transform.forward.normalized;
            Vector3 splineRight = splineAnimate.transform.right.normalized;

            // Compute target position with diagonal movement
            Vector3 targetPosition = splinePosition 
                + splineRight * laneOffset
                + splineForward * forwardOffsetFactor;

            // Smoothly move toward target
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeedSideways * Time.deltaTime
            );
        }
    }

    private void HandleLaneInput()
    {
        // Handle lane switching input
        if (positionIndex == 0 && Input.GetKeyDown(KeyCode.RightArrow)) 
            positionIndex = 1;
        else if (positionIndex == 2 && Input.GetKeyDown(KeyCode.LeftArrow))
            positionIndex = 1;
        else if (positionIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                positionIndex = 2;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) 
            {
                positionIndex = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tree") && !isSlowingDown)
        {
            Debug.Log("Hit a tree! Slowing down.");
            isSlowingDown = true;
            if (activateSpline != null)
            {
                activateSpline.targetSpeed = 18f; // Reduce speed on tree hit
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tree") && isSlowingDown)
        {
            if (activateSpline != null)
            {
                activateSpline.targetSpeed = 18f;
            }
                
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tree") && isSlowingDown)
        {
            Debug.Log("Left the tree!");
            isSlowingDown = false;
            if (activateSpline != null)
            {
                activateSpline.targetSpeed = 30f; // Reset speed
            }
        }
    }
}


