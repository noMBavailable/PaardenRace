using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LeaderboardManager : MonoBehaviour
{
    string addScoreURL = "https://localhost/addscore.php";
    string getLeaderboardURL = "https://localhost/getleaderboard.php";

    public IEnumerator UploadScore(string playerName, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", playerName);
        form.AddField("score", score);

        using (UnityWebRequest www = UnityWebRequest.Post(addScoreURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
                Debug.Log("Score uploaded successfully");
            else
                Debug.LogError("Error uploading score: " + www.error);
        }
    }
    public IEnumerator GetLeaderboard()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(getLeaderboardURL))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string[] entries = www.downloadHandler.text.Split('\n');
                foreach (string entry in entries)
                {
                    if (string.IsNullOrEmpty(entry)) continue;
                    string[] parts = entry.Split('|');
                    Debug.Log($"Name: {parts[0]}, Score: {parts[1]}");
                }
            }
            else
            {
                Debug.LogError("Error getting leaderboard: " + www.error);
            }
        }
    }
}