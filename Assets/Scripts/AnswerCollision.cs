using UnityEngine;

using UnityEngine;

public class AnswerCollision : MonoBehaviour
{
    [SerializeField] private string Answer;
    private CheckAnswer checkAnswer;
    private Renderer objectRenderer;    

    void Start()
    {
        checkAnswer = FindObjectOfType<CheckAnswer>();
        objectRenderer = GetComponent<Renderer>(); // Get the object's Renderer
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (checkAnswer.IsCorrect(Answer))
            {
                Debug.Log("Correct answer!");
                objectRenderer.material.color = Color.green;
            }
            else
            {
                Debug.Log("Wrong answer!");
                objectRenderer.material.color = Color.red;
            }
        }
    }
}
