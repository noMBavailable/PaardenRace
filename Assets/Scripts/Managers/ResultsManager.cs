using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelFinishLevel;
    public GameObject panelLeaderboard;

    [Header("Finish Level UI")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;
    public Button leaderboardButton;

    [Header("Leaderboard UI")]
    public TextMeshProUGUI leaderboardText;
    public Button resetButton;
    public Button levelButton;

    [Header("Admin Edit Leaderboard UI")]
    public GameObject editLeaderboardPanel;
    public Transform leaderboardListParent; // content area of ScrollView
    public Button entryButtonTemplate;
    public Button closeEditButton;
    public GameObject panelBetween;

    [Header("Admin Panel Reference")]
    public GameObject adminPanel;

    private List<(int id, string name, int score)> scores = new List<(int, string, int)>();

    void Start()
    {
        ShowFinishPanel();

        if (leaderboardButton != null)
            leaderboardButton.onClick.AddListener(ShowLeaderboardPanel);
        if (resetButton != null)
            resetButton.onClick.AddListener(ResetGame);
        if (levelButton != null)
            levelButton.onClick.AddListener(GoToLevels);
        if (closeEditButton != null)
            closeEditButton.onClick.AddListener(CloseEditLeaderboard);
    }

    // -------------------------
    // Panel Management
    // -------------------------
    private void ShowFinishPanel()
    {
        if (panelFinishLevel != null)
            panelFinishLevel.SetActive(true);
        if (panelLeaderboard != null)
            panelLeaderboard.SetActive(false);

        // Display formatted level
        string formattedLevel = FormatLevelName(GameSettings.Level);
        levelText.text = $"Finished Level: {formattedLevel}";

        nameText.text = $"Name: {GameSettings.PlayerName ?? "Unknown"}";
        scoreText.text = $"Score: {GameSettings.Score}";

        // Upload score to server
        StartCoroutine(LeaderboardAPI.UploadScore(GameSettings.PlayerName, GameSettings.Score, result =>
        {
            Debug.Log("✅ Upload result: " + result);
        }));
    }

    private string FormatLevelName(string level)
    {
        if (string.IsNullOrWhiteSpace(level))
            return "Unknown";

        // Example mapping, adjust to your level names
        return level switch
        {
            "Level 1" => "Forest",
            "Level 2" => "Desert",
            "Level 3" => "Castle",
            _ => level
        };
    }

    private void ShowLeaderboardPanel()
    {
        if (panelFinishLevel != null)
            panelFinishLevel.SetActive(false);
        if (panelLeaderboard != null)
            panelLeaderboard.SetActive(true);

        StartCoroutine(LeaderboardDownloader.GetLeaderboardCoroutine(downloadedScores =>
        {
            scores = downloadedScores;
            DisplayScores();
        }));
    }

    private void DisplayScores()
    {
        if (leaderboardText == null)
            return;

        leaderboardText.text = "Leaderboard\n\n";

        if (scores.Count == 0)
        {
            leaderboardText.text += "No scores yet!";
            return;
        }

        foreach (var s in scores)
        {
            leaderboardText.text += $"{s.name} - {s.score}\n";
        }
    }

    // -------------------------
    // Buttons
    // -------------------------
    private void ResetGame()
    {
        GameSettings.ResetAll();
        SceneManager.LoadScene("Main Menu");
    }

    private void GoToLevels()
    {
        GameSettings.ResetScore();
        SceneManager.LoadScene("Main Menu");
    }

    public void OpenEditLeaderboard()
    {
        StartCoroutine(LeaderboardDownloader.GetLeaderboardCoroutine(downloadedScores =>
        {
            scores = downloadedScores;

            if (adminPanel != null)
                adminPanel.SetActive(false);
            if (panelBetween != null)
                panelBetween.SetActive(true);

            // Clear old buttons
            foreach (Transform child in leaderboardListParent)
                Destroy(child.gameObject);

            // Populate new list
            for (int i = 0; i < scores.Count; i++)
            {
                int index = i; // capture for lambda
                var entry = Instantiate(entryButtonTemplate, leaderboardListParent);
                entry.gameObject.SetActive(true);

                TextMeshProUGUI txt = entry.GetComponentInChildren<TextMeshProUGUI>();
                if (txt != null)
                    txt.text = $"{scores[i].name} - {scores[i].score}";

                entry.onClick.AddListener(() =>
                {
                    Debug.Log("Admin deletion not implemented on server.");
                });
            }

            editLeaderboardPanel.transform.SetAsLastSibling();
            editLeaderboardPanel.SetActive(true);
        }));
    }

    public void CloseEditLeaderboard()
    {
        if (editLeaderboardPanel != null)
            editLeaderboardPanel.SetActive(false);
        if (adminPanel != null)
            adminPanel.SetActive(true);
    }
}
