using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardDownloader : MonoBehaviour
{
    public static string leaderboardURL = "http://localhost/MuseumProject/getleaderboard.php";

    public static IEnumerator GetLeaderboardCoroutine(System.Action<List<(int id, string name, int score)>> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(leaderboardURL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Leaderboard download error: " + www.error);
            callback(new List<(int, string, int)>());
            yield break;
        }

        string data = www.downloadHandler.text;
        Debug.Log("✅ Leaderboard data received:\n" + data);

        List<(int id, string name, int score)> scores = new List<(int, string, int)>();

        string[] lines = data.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Trim().Split('|');
            if (parts.Length < 3)
                continue;

            if (!int.TryParse(parts[0], out int id))
                continue;

            string playerName = parts[1];

            if (!int.TryParse(parts[2], out int score))
                continue;

            scores.Add((id, playerName, score));
        }

        callback(scores);
    }
}