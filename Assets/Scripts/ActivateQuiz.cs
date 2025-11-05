using UnityEngine;

public class ActivateQuiz : MonoBehaviour
{

    [SerializeField] private GameObject AnswerA;
    [SerializeField] private GameObject AnswerB;
    [SerializeField] private GameObject AnswerC;
    [SerializeField] private GameObject QuizCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        enable blocks
        enable canvas
        
        
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AnswerA.SetActive(true);
            AnswerB.SetActive(true);
            AnswerC.SetActive(true);
            QuizCanvas.SetActive(true);
        }
    }
}
