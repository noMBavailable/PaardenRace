using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public static class LocalizationManager
{
    public static string CurrentLanguage = "English";

    private static Dictionary<string, Dictionary<string, string>> translations;

    public static void Initialize()
    {
        translations = new Dictionary<string, Dictionary<string, string>>()
        {
            // Basic UI
            {"StartButton", new(){{"English", "Start Game"}, {"Nederlands", "Start Spel"}, {"Frysk", "Start Spul"}}},
            {"NextButton", new(){{"English", "Next"}, {"Nederlands", "Volgende"}, {"Frysk", "Folgjende"}}},
            {"BackButton", new(){{"English", "Back"}, {"Nederlands", "Terug"}, {"Frysk", "Werom"}}},
            {"PlayerLabel", new(){{"English", "Player: "}, {"Nederlands", "Speler: "}, {"Frysk", "Spiler: "}}},

            // Colorblind modes
            {"ColorblindModes_Normal", new(){{"English", "Normal"}, {"Nederlands", "Normaal"}, {"Frysk", "Normaal"}}},
            {"ColorblindModes_Protanopia", new(){{"English", "Protanopia"}, {"Nederlands", "Protanopie"}, {"Frysk", "Protanopy"}}},
            {"ColorblindModes_Deuteranopia", new(){{"English", "Deuteranopia"}, {"Nederlands", "Deuteranopie"}, {"Frysk", "Deuteranopy"}}},
            {"ColorblindModes_Tritanopia", new(){{"English", "Tritanopia"}, {"Nederlands", "Tritanopie"}, {"Frysk", "Tritanopy"}}},

            // Difficulties
            {"Difficulties_Easy", new(){{"English", "Easy"}, {"Nederlands", "Makkelijk"}, {"Frysk", "Maklik"}}},
            {"Difficulties_Medium", new(){{"English", "Medium"}, {"Nederlands", "Gemiddeld"}, {"Frysk", "Gemiddeld"}}},
            {"Difficulties_Hard", new(){{"English", "Hard"}, {"Nederlands", "Moeilijk"}, {"Frysk", "Dreech"}}},

            // Levels
            {"Levels_Level1", new(){{"English", "Level 1"}, {"Nederlands", "Niveau 1"}, {"Frysk", "Nivo 1"}}},
            {"Levels_Level2", new(){{"English", "Level 2"}, {"Nederlands", "Niveau 2"}, {"Frysk", "Nivo 2"}}},
            {"Levels_Level3", new(){{"English", "Level 3"}, {"Nederlands", "Niveau 3"}, {"Frysk", "Nivo 3"}}},
        };
    }

    public static string Get(string key)
    {
        if (translations != null && translations.ContainsKey(key) && translations[key].ContainsKey(CurrentLanguage))
            return translations[key][CurrentLanguage];
        return key;
    }

    public static void UpdateButtonTexts(Button[] buttons, string category)
    {
        if (buttons == null) return;
        foreach (Button btn in buttons)
        {
            TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (txt == null) continue;

            string lookupKey = $"{category}_{txt.text.Replace(" ", "")}";
            if (translations.ContainsKey(lookupKey))
                txt.text = Get(lookupKey);
        }
    }
}
