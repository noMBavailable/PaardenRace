using UnityEngine;
using System.Collections; 
using UnityEngine.Splines;

public class ActivateSpline : MonoBehaviour
{
    [SerializeField] private SplineAnimate splineAnimate;
    [SerializeField] private float acceleration = 3f;
    [SerializeField] public float targetSpeed = 0f;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField]private bool isPlaying = false;

    void Start()
    {
        splineAnimate.Pause();
        splineAnimate.NormalizedTime = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            if (!isPlaying)
            {
                isPlaying = true;
                targetSpeed = 30f;
            }
            else if(isPlaying)
            {
                Debug.Log("Speed boost!");
                targetSpeed = 50f;
                StartCoroutine(ResetSpeedCoroutine(5f));
            }


        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            targetSpeed = 20f;
        }

        // Smoothly change current speed (physically)
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        if (isPlaying)
        {
            // Move along spline manually by normalized distance
            float distancePerSecond = currentSpeed * Time.deltaTime;
            splineAnimate.NormalizedTime += distancePerSecond / splineAnimate.Container.Spline.GetLength();
            splineAnimate.NormalizedTime = Mathf.Clamp01(splineAnimate.NormalizedTime);
        }
    }
    
    private IEnumerator ResetSpeedCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset target speed to your base value (for example, 30f)
        targetSpeed = 30f;

        Debug.Log("Speed reset to normal after delay!");
    }
    
}



