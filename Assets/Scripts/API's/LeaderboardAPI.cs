using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public static class LeaderboardAPI
{
    private static string baseUrl = "http://localhost/MuseumProject/";

    // --------------------------
    // ✅ Upload Score
    // --------------------------
    public static IEnumerator UploadScore(string name, int score, Action<string> callback)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Debug.LogError("❌ Player name is empty — cancelling upload");
            callback?.Invoke("error");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("score", score);

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + "addscore.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("❌ Upload failed: " + www.error);
                callback?.Invoke("error");
            }
            else
            {
                Debug.Log("✅ Score uploaded: " + www.downloadHandler.text);
                callback?.Invoke("success");
            }
        }
    }

    // --------------------------
    // ✅ Download Leaderboard
    // --------------------------
    public static IEnumerator DownloadScores(Action<List<(int id, string name, int score)>> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(baseUrl + "getleaderboard.php"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("❌ Error getting leaderboard: " + www.error);
                callback(new List<(int, string, int)>());
                yield break;
            }

            List<(int id, string name, int score)> scores = new List<(int, string, int)>();

            string[] lines = www.downloadHandler.text.Split('\n');

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split('|');
                if (parts.Length < 3) continue;

                if (!int.TryParse(parts[0], out int id)) continue;
                string name = parts[1];
                if (!int.TryParse(parts[2], out int score)) continue;

                scores.Add((id, name, score));
            }

            callback(scores);
        }
    }

    // --------------------------
    // ✅ Delete Score (Admin)
    // --------------------------
    public static IEnumerator DeleteScore(int id, Action<bool> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + "deletescore.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success && www.downloadHandler.text.Contains("success"))
            {
                callback(true);
            }
            else
            {
                callback(false);
            }
        }
    }
}