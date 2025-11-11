using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LeaderboardUploader : MonoBehaviour
{
    [SerializeField] string addScoreURL = "http://localhost/MuseumProject/addscore.php";

    public void UploadScore(string playerName, int score)
    {
        StartCoroutine(UploadScoreCoroutine(playerName, score));
    }

    IEnumerator UploadScoreCoroutine(string playerName, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", playerName);
        form.AddField("score", score);

        using (UnityWebRequest www = UnityWebRequest.Post(addScoreURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ Score uploaded successfully: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("❌ Upload failed: " + www.error);
            }
        }
    }
}
