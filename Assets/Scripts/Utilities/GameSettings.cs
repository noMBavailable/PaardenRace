public static class GameSettings
{
    public static string Language = "English";
    public static string PlayerName;
    public static string ColorblindMode = "Normal";
    public static string Level;
    public static string Difficulty = "Normal";
    public static int Score;
    public static int CurrentPanelIndex = 0;
    public static bool GoToLevelsPanel = false;

    public static void ResetAll()
    {
        Language = "English";
        PlayerName = "";
        ColorblindMode = "Normal";
        Level = "";
        Difficulty = "Normal";
        Score = 0;
        CurrentPanelIndex = 0;
        GoToLevelsPanel = false;
    }

    public static void ResetScore()
    {
        Score = 0;
        GoToLevelsPanel = true;
    }
}