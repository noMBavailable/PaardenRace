using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Panels (in order)")]
    public GameObject[] panels; // 0: Language, 1: Name, 2: Colorblind, 3: Level, 4: Difficulty

    [Header("Navigation Buttons")]
    public Button backButton;
    public Button nextButton;
    public Button startButton;
    public TextMeshProUGUI startButtonText;
    public Image startButtonBackground;
    public GameObject navigationObject; // parent of navigation buttons

    [Header("Level Buttons")]
    public Button[] levelButtons;

    [Header("Difficulty Buttons")]
    public Button[] difficultyButtons;

    [Header("Selected Colors")]
    public Color selectedColor = Color.green;
    public Color defaultColor = Color.white;

    [Header("Button Colors")]
    public Color hoverColor = new Color(0.8f, 0.8f, 0.8f); // Slight grey for hover

    [Header("Player Name Display")]
    public TextMeshProUGUI playerNameText;

    [Header("Admin UI")]
    public GameObject adminLoginPanel;
    public GameObject adminPanel;
    public GameObject panelBetween;
    public GameObject languagePanel;

    [Header("Admin Login Fields")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text loginErrorText;
    public Button loginButton;
    public Button logoutButton;
    public Button restartButton;
    public Button adminLoginBackButton;

    private int currentPanel = 0;
    private bool isLoggedIn = false;

    void Start()
    {
        int startPanel = GameSettings.GoToLevelsPanel ? 3 : GameSettings.CurrentPanelIndex;
        ShowPanel(startPanel);
        UpdateNavigationButtons();
        GameSettings.GoToLevelsPanel = false;

        if (!string.IsNullOrWhiteSpace(GameSettings.Difficulty))
            UpdateButtonColors(difficultyButtons, GameSettings.Difficulty);

        // Hide admin UI
        if (adminPanel != null) adminPanel.SetActive(false);
        if (adminLoginPanel != null) adminLoginPanel.SetActive(false);
        if (panelBetween != null) panelBetween.SetActive(false);
        if (loginErrorText != null) loginErrorText.text = "";

        // Admin button hooks
        if (loginButton != null) loginButton.onClick.AddListener(AttemptLogin);
        if (logoutButton != null) logoutButton.onClick.AddListener(LogoutAdmin);
        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (adminLoginBackButton != null) adminLoginBackButton.onClick.AddListener(CloseAdminLoginPanel);
    }

    // -------------------------
    // Panel Navigation
    // -------------------------
    public void ShowPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(i == index);

        currentPanel = index;
        UpdateNavigationButtons();
        UpdatePlayerNameDisplay();

        if (CursorManager.Instance != null)
            CursorManager.Instance.SetDefaultCursor();

        GameSettings.CurrentPanelIndex = index;
    }

    public void NextPanel()
    {
        if (currentPanel == 1)
            Debug.Log($"🔄 Player Name confirmed: {GameSettings.PlayerName}");

        if (currentPanel < panels.Length - 1)
            ShowPanel(currentPanel + 1);
    }

    public void PreviousPanel()
    {
        if (currentPanel > 0)
            ShowPanel(currentPanel - 1);
    }

    void UpdateNavigationButtons()
    {
        if (backButton != null)
            backButton.gameObject.SetActive(currentPanel > 0);

        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(currentPanel < panels.Length - 1);
            nextButton.interactable = IsCurrentPanelValid();
        }

        if (startButton != null)
            startButton.gameObject.SetActive(currentPanel == panels.Length - 1);

        ValidateStartButton();
    }

    private bool IsCurrentPanelValid()
    {
        switch (currentPanel)
        {
            case 0: return !string.IsNullOrWhiteSpace(GameSettings.Language);
            case 1: return !string.IsNullOrWhiteSpace(GameSettings.PlayerName);
            case 2: return !string.IsNullOrWhiteSpace(GameSettings.ColorblindMode);
            case 3: return !string.IsNullOrWhiteSpace(GameSettings.Level);
            case 4: return !string.IsNullOrWhiteSpace(GameSettings.Difficulty);
            default: return false;
        }
    }

    private void UpdatePlayerNameDisplay()
    {
        if (playerNameText == null) return;

        if (currentPanel >= 2 && !string.IsNullOrWhiteSpace(GameSettings.PlayerName))
        {
            playerNameText.gameObject.SetActive(true);
            playerNameText.text = "Player: " + GameSettings.PlayerName;
        }
        else
        {
            playerNameText.gameObject.SetActive(false);
        }
    }

    // -------------------------
    // UI Binding Methods
    // -------------------------
    public void SetLanguage(int index)
    {
        string[] langs = { "English", "Nederlands", "Frysk" };
        if (index >= 0 && index < langs.Length)
        {
            string previous = GameSettings.Language;
            GameSettings.Language = langs[index];
            if (previous != GameSettings.Language)
                Debug.Log($"🔄 Language changed: {previous ?? "None"} → {GameSettings.Language}");
        }
        UpdateNavigationButtons();
    }

    public void NameInputChanged(string name)
    {
        GameSettings.PlayerName = (name ?? "").Trim();

        if (currentPanel == 1 && nextButton != null)
            nextButton.interactable = !string.IsNullOrWhiteSpace(GameSettings.PlayerName);
    }

    public void SetPlayerName(string name)
    {
        string previous = GameSettings.PlayerName;
        GameSettings.PlayerName = (name ?? "").Trim();
        if (previous != GameSettings.PlayerName)
            Debug.Log($"🔄 Player Name changed: {previous ?? "None"} → {GameSettings.PlayerName}");

        UpdatePlayerNameDisplay();
        UpdateNavigationButtons();
    }

    public void SetColorblindMode(int index)
    {
        string[] modes = { "Normal", "Protanopia", "Deuteranopia", "Tritanopia" };
        if (index >= 0 && index < modes.Length)
        {
            string previous = GameSettings.ColorblindMode;
            GameSettings.ColorblindMode = modes[index];
            if (previous != GameSettings.ColorblindMode)
                Debug.Log($"🔄 Colorblind mode changed: {previous ?? "None"} → {GameSettings.ColorblindMode}");
        }
        UpdateNavigationButtons();
    }

    public void SelectLevel(string levelName)
    {
        GameSettings.Level = levelName;
        Debug.Log($"🔄 Level changed → {GameSettings.Level}");
        UpdateButtonColors(levelButtons, levelName);
        UpdateNavigationButtons();
    }

    public void SelectDifficulty(string difficulty)
    {
        GameSettings.Difficulty = difficulty;
        Debug.Log($"🔄 Difficulty changed → {GameSettings.Difficulty}");
        UpdateButtonColors(difficultyButtons, difficulty);
        UpdateNavigationButtons();
    }

    private void UpdateButtonColors(Button[] buttons, string selectedName)
    {
        foreach (Button btn in buttons)
        {
            if (btn == null) continue;

            TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (txt == null) continue;

            bool isSelected = txt.text == selectedName;

            ColorBlock cb = btn.colors;
            cb.normalColor = isSelected ? selectedColor : defaultColor;
            cb.highlightedColor = isSelected ? selectedColor : hoverColor;
            cb.pressedColor = isSelected ? selectedColor : hoverColor;
            cb.selectedColor = selectedColor;
            btn.colors = cb;
            btn.image.color = cb.normalColor;
        }
    }

    // -------------------------
    // Admin Panel / Login Logic
    // -------------------------
    public void OpenAdminPanel()
    {
        if (panelBetween != null) panelBetween.SetActive(true);
        if (languagePanel != null) SetPanelInteractable(languagePanel, false);
        if (navigationObject != null) navigationObject.SetActive(false);

        if (isLoggedIn)
        {
            if (adminPanel != null) adminPanel.SetActive(true);
        }
        else
        {
            if (adminLoginPanel != null) adminLoginPanel.SetActive(true);
            if (usernameInput != null) usernameInput.text = "";
            if (passwordInput != null) passwordInput.text = "";
            if (loginErrorText != null) loginErrorText.text = "";
        }
    }

    private void AttemptLogin()
    {
        string username = usernameInput != null ? usernameInput.text.Trim() : "";
        string password = passwordInput != null ? passwordInput.text.Trim() : "";

        if (username == "admin" && password == "admin")
        {
            Debug.Log("✅ Admin login successful.");
            isLoggedIn = true;

            if (adminLoginPanel != null) adminLoginPanel.SetActive(false);
            if (adminPanel != null) adminPanel.SetActive(true);
            if (loginErrorText != null) loginErrorText.text = "";
        }
        else
        {
            Debug.LogWarning("❌ Invalid admin credentials.");
            if (loginErrorText != null) loginErrorText.text = "Invalid username and/or password.";
        }
    }

    private void CloseAdminLoginPanel()
    {
        if (adminLoginPanel != null) adminLoginPanel.SetActive(false);
        if (panelBetween != null) panelBetween.SetActive(false);
        if (languagePanel != null) SetPanelInteractable(languagePanel, true);
        if (navigationObject != null) navigationObject.SetActive(true);

        if (usernameInput != null) usernameInput.text = "";
        if (passwordInput != null) passwordInput.text = "";
        if (loginErrorText != null) loginErrorText.text = "";

        isLoggedIn = false;
        if (CursorManager.Instance != null) CursorManager.Instance.SetDefaultCursor();
    }

    private void LogoutAdmin()
    {
        isLoggedIn = false;

        if (adminPanel != null) adminPanel.SetActive(false);
        if (adminLoginPanel != null) adminLoginPanel.SetActive(true);

        if (usernameInput != null) usernameInput.text = "";
        if (passwordInput != null) passwordInput.text = "";
        if (loginErrorText != null) loginErrorText.text = "";

        if (panelBetween != null) panelBetween.SetActive(true);
        if (languagePanel != null) SetPanelInteractable(languagePanel, false);
        if (navigationObject != null) navigationObject.SetActive(false);

        if (CursorManager.Instance != null) CursorManager.Instance.SetDefaultCursor();
    }

    private void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void ClearLeaderboard()
    {
        Debug.Log("🧹 Attempting to clear leaderboard from server...");
        StartCoroutine(ClearLeaderboardServer());
    }

    private IEnumerator ClearLeaderboardServer()
    {
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get("http://localhost/MuseumProject/clearleaderboard.php"))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
                Debug.Log("Leaderboard cleared on server.");
            else
                Debug.LogError("Failed to clear leaderboard: " + www.error);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetPanelInteractable(GameObject panel, bool interactable)
    {
        Button[] buttons = panel.GetComponentsInChildren<Button>();
        foreach (Button btn in buttons) btn.interactable = interactable;
    }

    public void StartGame()
    {
        bool valid = !string.IsNullOrWhiteSpace(GameSettings.Language)
                     && !string.IsNullOrWhiteSpace(GameSettings.PlayerName)
                     && !string.IsNullOrWhiteSpace(GameSettings.ColorblindMode)
                     && !string.IsNullOrWhiteSpace(GameSettings.Level)
                     && !string.IsNullOrWhiteSpace(GameSettings.Difficulty);

        if (!valid) return;
        Debug.Log("🎉 All requirements met! Starting the game...");
        SceneManager.LoadScene("Game");
    }

    private void ValidateStartButton()
    {
        if (startButton == null) return;

        bool valid = !string.IsNullOrWhiteSpace(GameSettings.Language)
                     && !string.IsNullOrWhiteSpace(GameSettings.PlayerName)
                     && !string.IsNullOrWhiteSpace(GameSettings.ColorblindMode)
                     && !string.IsNullOrWhiteSpace(GameSettings.Level)
                     && !string.IsNullOrWhiteSpace(GameSettings.Difficulty);

        startButton.interactable = valid;

        if (startButtonText != null)
            startButtonText.text = valid ? "Start Game" : "Start Game";

        if (startButtonBackground != null)
            startButtonBackground.color = valid ? Color.green : new Color(0.6f, 0.6f, 0.6f, 1f);
    }
}