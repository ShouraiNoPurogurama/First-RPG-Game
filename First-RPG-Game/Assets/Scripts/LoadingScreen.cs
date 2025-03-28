using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingCanvas;
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;

    private void Awake()
    {
        loadingCanvas.SetActive(false);
        Destroy(gameObject);
    }
    /// <summary>
    /// Show the loading screen
    /// </summary>
    public void ShowLoadingScreen()
    {
        loadingCanvas.SetActive(true);
        loadingBar.value = 0;
        if (loadingText != null) loadingText.text = "0%";
    }
    /// <summary>
    /// Update progress of the loading screen
    /// </summary>
    /// <param name="progress"></param>
    public void UpdateLoadingProgress(float progress)
    {
        loadingBar.value = progress;
        if (loadingText != null) loadingText.text = Mathf.RoundToInt(progress * 100f) + "%";
    }
    /// <summary>
    /// Hide the loading screen by setting it to inactive
    /// </summary>
    public void HideLoadingScreen()
    {
        loadingCanvas.SetActive(false);
    }
}
