using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlaceholder : MonoBehaviour
{
    public Button finishLevelButton;
    public Button scoreButton;

    void Start()
    {
        finishLevelButton.onClick.AddListener(() => SceneManager.LoadScene("Results"));
        scoreButton.onClick.AddListener(() => GameSettings.Score += 10);
    }
}
