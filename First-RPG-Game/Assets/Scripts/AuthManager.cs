using Save_and_Load;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class AuthManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject loginPanel;
    public GameObject registerPanel;
    public AuthManager instance;

    [Header("Login Fields")]
    public TMP_InputField loginUsernameInput;
    public TMP_InputField loginPasswordInput;
    public TMP_Text loginResponseText;

    [Header("Register Fields")]
    public TMP_InputField registerUsernameInput;
    public TMP_InputField registerPasswordInput;
    public TMP_Text registerResponseText;

    private string registerUrl = "http://prn-222.food/api/account/register";
    private string loginUrl = "http://prn-222.food/api/account/login";
    private void Start()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }
    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void ShowLoginPanel()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void Register()
    {
        string username = registerUsernameInput.text;
        string password = registerPasswordInput.text;
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            registerResponseText.text = "Please fill in all the information!";
            return;
        }
        StartCoroutine(RegisterCoroutine(username, password));
    }

    public void Login()
    {
        Debug.Log("Login button clicked!");
        string username = loginUsernameInput.text;
        string password = loginPasswordInput.text;
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            loginResponseText.text = "Please fill in all the information!";
            return;
        }
        StartCoroutine(LoginCoroutine(username, password));
    }
    public class ApiResponse
    {
        public string message;
    }

    private IEnumerator RegisterCoroutine(string username, string password)
    {
        string json = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(registerUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                registerResponseText.text = "Register Successfully! Now you can go to Login Page";
                //ShowLoginPanel(); 
            }
            else
            {
                string responseText = request.downloadHandler.text;
                ApiResponse apiResponse = JsonUtility.FromJson<ApiResponse>(responseText);

                if (apiResponse != null && !string.IsNullOrEmpty(apiResponse.message))
                {
                    registerResponseText.text = "Err: " + apiResponse.message;
                }
                else
                {
                    registerResponseText.text = "Register Failed! Please try again";
                }
            }
        }
    }

    private IEnumerator LoginCoroutine(string username, string password)
    {
        string json = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(loginUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string tokenRaw = request.downloadHandler.text;
                var actualToken = tokenRaw.Split(":").Last();

                actualToken = actualToken.Replace("\"", " ");
                actualToken = actualToken.Replace("}", " ");
                actualToken = actualToken.Trim();
                Debug.Log("Authorization:  Bearer raw token " + tokenRaw);
                Debug.Log("ACTUAL TOKEN: " + actualToken);

                //token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJ1c2VybmFtZSI6ImRhdGhsZWNueCIsImV4cCI6MTc0MjQ0MDQyNSwiaXNzIjoiUlBHLUFQSSIsImF1ZCI6IlVuaXR5R2FtZUNsaWVudCJ9.hS3iYtZ2aR9UVwXfcpbZiSr-J4sLIlVqsIc7OvOJylo";

                loginResponseText.text = "Login Successfully !";
                yield return new WaitForSeconds(1);

                //SceneManager.
                //("Level2");
                PlayerPrefs.SetString("authToken", actualToken);
                yield return StartCoroutine(LoadUserData(username, actualToken));
                //yield return StartCoroutine(checktoken(username, actualToken));

            }
            else
            {
                string responseText = request.downloadHandler.text;
                ApiResponse apiResponse = JsonUtility.FromJson<ApiResponse>(responseText);

                if (apiResponse != null && !string.IsNullOrEmpty(apiResponse.message))
                {
                    loginResponseText.text = "Error: " + apiResponse.message;
                }
                else
                {
                    loginResponseText.text = "Login Failed! Please check username/password";
                }
            }
        }
    }
    private IEnumerator LoadUserData(string username, string token)
    {
        string apiUrl = $"http://prn-222.food/api/v1/Data?username={username}";

        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResult = request.downloadHandler.text;
                GameData gameData = JsonUtility.FromJson<GameData>(jsonResult);

                if (gameData != null)
                {
                    Debug.Log("🟢 Đăng nhập thành công, chuyển đến scene: MainMenu");
                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    Debug.LogError("🔴 Lỗi: Không thể đọc dữ liệu người chơi!");
                }
            }
            else
            {
                Debug.LogError("🔴 Lỗi lấy dữ liệu người chơi: " + request.responseCode + " - " + request.downloadHandler.text);
            }
        }
    }

    private IEnumerator checktoken(string username, string token)
    {
        string apiUrl = $"http://prn-222.food/api/v1/Data?username={username}";
        Debug.Log("🟢 API URL: " + apiUrl);
        Debug.Log("🟢 Token gửi đi: [" + token + "]");
        Debug.Log(token);
        // Kiểm tra token có hợp lệ không
        if (string.IsNullOrWhiteSpace(token))
        {
            Debug.LogError("🔴 Token bị null hoặc trống!");
            yield break; // Thoát coroutine nếu token lỗi
        }

        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {

            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.SetRequestHeader("Accept", "application/json");

            // Debug headers
            Debug.Log("🟢 Headers gửi đi:");
            //Debug.Log("Authorization: Bearer " + token);
            Debug.Log("Accept: application/json");

            yield return request.SendWebRequest();

            // Debug response
            Debug.Log("🔵 Response Code: " + request.responseCode);
            Debug.Log("🔵 Response Body: " + request.downloadHandler.text);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("🟢 API trả về thành công!");
            }
            else if (request.responseCode == 401)
            {
                Debug.LogError("🔴 Lỗi 401 - Unauthorized: Token không hợp lệ hoặc hết hạn!");
            }
            else
            {
                Debug.LogError("🔴 Lỗi lấy dữ liệu: " + request.responseCode + " - " + request.downloadHandler.text);
            }
        }
    }



}
