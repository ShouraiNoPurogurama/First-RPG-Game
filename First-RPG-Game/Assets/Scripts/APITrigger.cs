using MainCharacter;
using Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class RubyResponse
{
    public int ruby;
}
public class APITrigger : MonoBehaviour
{
    private string apiUrl = "http://prn-222.food/api/v1/data/ruby"; 
    private string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJ1c2VybmFtZSI6ImRhdGhsZWNueCIsImV4cCI6MTc0MjQ0OTIyMiwiaXNzIjoiUlBHLUFQSSIsImF1ZCI6IlVuaXR5R2FtZUNsaWVudCJ9.wLeuoUL5ek9ulCS8AgIDUEAJxf2scg3_Ov00-bV_tJ4"; // Lấy token hợp lệ
    public static APITrigger Instance;
    private void Awake()
    {
        //When have multiple scenes
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        token = PlayerPrefs.GetString("authToken");
    }

    public async Task LoadRubyDB()
    {

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "GET"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.SetRequestHeader("accept", "*/*");

            // Gửi request & chờ phản hồi
            await request.SendWebRequest();

            // Kiểm tra kết quả
            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                RubyResponse rubyData = JsonUtility.FromJson<RubyResponse>(jsonResponse);
                PlayerManager.Instance.player.GetComponent<PlayerStats>().Ruby = rubyData.ruby;
            }
            else
            {
                Debug.LogError("🔴 Lỗi khi gửi API: " + request.error);
            }
        }

    }

    public async Task SaveRuby()
    {
        var ruby = GameObject.Find("Player").GetComponent<PlayerStats>().Ruby;

        // Tạo URL với query string
        string urlWithParams = $"{apiUrl}?ruby={ruby}";

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(urlWithParams, ""))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.SetRequestHeader("accept", "*/*");

            // Gửi request & chờ phản hồi
            await request.SendWebRequest();

            // Kiểm tra kết quả
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("🟢 Dữ liệu gửi thành công: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("🔴 Lỗi khi gửi API: " + request.error);
            }
        }
    }

}
