
using UnityEngine;
using UnityEngine.Splines;
using System.Collections; 

public class SplineTest : MonoBehaviour
{

     [Header("Lane movement")]
    [SerializeField] private float moveAmount = 1.0f;          // Distance between lanes
    [SerializeField] private float moveSpeedSideways = 5.0f;  // How fast to slide
    private int positionIndex = 1;                             // Current lane (0 = left, 1 = middle, 2 = right)
    private float laneOffset = 0f;                             // X offset from spline
    [SerializeField] private SplineAnimate splineAnimate;
    private ActivateSpline activateSpline;

    private bool isSlowingDown = false;

    // Adjustable forward factor for diagonal movement
    [SerializeField] private float forwardOffsetFactor = 0.5f;

    [Header("Test")]
    [SerializeField] private SplineAnimate[] splines; // 0 = left, 1 = middle, 2 = right

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activateSpline = FindObjectOfType<ActivateSpline>();
    }

    // Update is called once per frame
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







    



/*

using UnityEngine;
using UnityEngine.Splines;
using System.Collections; 

public class Movement : MonoBehaviour
{
    [Header("Lane movement")]
    [SerializeField] private float moveAmount = 1.0f;      // Distance between lanes
    [SerializeField] private float moveSpeedSideways = 5.0f;       // How fast to slide
    private int positionIndex = 1;                         // Current lane (0 = left, 1 = middle, 2 = right)
    private float laneOffset = 0f;                         // X offset from spline
    [SerializeField] private SplineAnimate splineAnimate;
    private ActivateSpline activateSpline;

    private bool isSlowingDown = false;

    void Start()
    {
        activateSpline = FindObjectOfType<ActivateSpline>();
        // if (activateSpline == null)
        // {
        //     Debug.LogError("ActivateSpline script not found in the scene.");
        // }
    }

    void Update()
    {

       
        // Handle lane switching input
        if (positionIndex == 0 && Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            positionIndex = 1;
        }
        else if (positionIndex == 2 && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            positionIndex = 1;
        }
        else if (positionIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                positionIndex = 2;
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) 
                positionIndex = 0;
        }

        // Calculate lane offset
        laneOffset = (positionIndex - 1) * moveAmount;

        // Move relative to spline
        if (splineAnimate != null)
        {
            Vector3 splinePosition = splineAnimate.transform.position;
            Vector3 targetPosition = splinePosition + new Vector3(laneOffset, 0, 0);
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeedSideways * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tree") && !isSlowingDown)
        {
            Debug.Log("Hit a tree! Slowing down.");
            isSlowingDown = true;
            activateSpline.targetSpeed = 18f; // Reduce speed on tree hit
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tree") && isSlowingDown)
        {
            Debug.Log("Hit a tree! stay");
            
            activateSpline.targetSpeed = 18f; // Reduce speed on tree hit
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tree") && isSlowingDown)
        {
            Debug.Log("Hit a tree! exit");
            isSlowingDown = false;
            activateSpline.targetSpeed = 30f; // Reduce speed on tree hit
        }
    }

}

*/