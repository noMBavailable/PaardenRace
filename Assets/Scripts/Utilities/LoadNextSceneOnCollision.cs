using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneOnCollision : MonoBehaviour
{
    // Alternative if you use Trigger instead of collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        // Load next scene
        SceneManager.LoadScene(nextScene);
    }
}