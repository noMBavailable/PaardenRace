using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.ComponentModel;

public class IdleResetManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject warningPanel;          // The panel containing the warning text
    public TextMeshProUGUI warningText;      // The TMP text showing the message

    [Header("Idle Settings")]
    public float warningTime = 30f;          // Seconds before showing warning
    public float resetTime = 60f;            // Seconds before resetting

    [Header("Read-Only Timer")]
    [SerializeField, ReadOnly(true)]
    private float idleTimer = 0f;            // Tracks idle time

    private void Start()
    {
        if (warningPanel != null)
            warningPanel.SetActive(false);  // Hide panel initially
    }

    private void Update()
    {
        // Do not start timer if player is on the first scene panel
        if (SceneManager.GetActiveScene().name == "Main Menu" &&
            GameSettings.CurrentPanelIndex == 0)
        {
            idleTimer = 0f;
            if (warningPanel != null) warningPanel.SetActive(false);
            return;
        }

        // Increment idle timer
        idleTimer += Time.deltaTime;

        // Show warning panel at warningTime
        if (idleTimer >= warningTime && warningPanel != null)
            warningPanel.SetActive(true);

        // Reset game at resetTime
        if (idleTimer >= resetTime)
            ResetGame();

        // Detect input/movement
        if (Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            idleTimer = 0f;
            if (warningPanel != null)
                warningPanel.SetActive(false);
        }

        // Optional: Update warning text dynamically (like countdown)
        if (warningText != null && idleTimer >= warningTime)
        {
            float remaining = resetTime - idleTimer;
            warningText.text = $"Game will reset in {Mathf.CeilToInt(remaining)} seconds if no input is detected!";
        }
    }

    private void ResetGame()
    {
        GameSettings.ResetAll(); // Resets all game data but keeps scores if desired
        SceneManager.LoadScene("Main Menu");
    }
}
