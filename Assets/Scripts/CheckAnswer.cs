using UnityEngine;

public class CheckAnswer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public bool IsCorrect(string answer)
    {
        // Placeholder logic for checking the answer
        // Replace this with your actual answer checking logic
        string correctAnswer = "A"; // Example correct answer
        return answer == correctAnswer;
    }
}
