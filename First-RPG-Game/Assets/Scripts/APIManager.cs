using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class LoginRequest
{
    public string username;
    public string password;
}
public class APIManager : MonoBehaviour
{
    private string apiUrl = "http://prn-222.food/api/account/login";

    void Start()
    {
        LoginRequest loginData = new LoginRequest
        {
            username = "dathlecnx",
            password = "123"
        };
        StartCoroutine(PostRequest(apiUrl, JsonUtility.ToJson(loginData)));
    }

    IEnumerator PostRequest(string url, string json)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}
