using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class AuthManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject loginPanel;
    public GameObject registerPanel;

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
        StartCoroutine(RegisterCoroutine(username, password));
    }

    public void Login()
    {
        Debug.Log("Login button clicked!");
        string username = loginUsernameInput.text;
        string password = loginPasswordInput.text;
        StartCoroutine(LoginCoroutine(username, password));
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
                registerResponseText.text = "Register Successfully!";
                ShowLoginPanel(); 
            }
            else
            {
                registerResponseText.text = "Err: " + request.downloadHandler.text;
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
                string token = request.downloadHandler.text; 
                loginResponseText.text = "  Login Successfully !";
                PlayerPrefs.SetString("authToken", token); 
            }
            else
            {
                loginResponseText.text = "Err: " + request.downloadHandler.text;
            }
        }
    }
}
