using UnityEngine;
using UnityEngine.UI;

public class OpenBrowser : MonoBehaviour
{
    public Button openButton; 
    public string url = "https://google.com";

    void Start()
    {
        openButton.onClick.AddListener(OpenWebPage);
    }

    void OpenWebPage()
    {
        Application.OpenURL(url);
    }
}
