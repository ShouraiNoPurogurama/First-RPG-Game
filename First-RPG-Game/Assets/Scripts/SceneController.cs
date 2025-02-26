using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    [Header("Loading Screen Reference")]
    public LoadingScreen loadingScreenInstance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Chuyển sang level tiếp theo dựa vào buildIndex của scene
    /// </summary>
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Load scene theo tên và hiển thị loading screen
    /// </summary>
    /// <param name="sceneName">Tên của scene cần load</param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        if (loadingScreenInstance != null)
            loadingScreenInstance.ShowLoadingScreen();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        float simulatedProgress = 0f;
        float speed = 0.3f;

        while (simulatedProgress < 0.9f)
        {
            simulatedProgress += Time.deltaTime * speed;
            float currentProgress = Mathf.Min(simulatedProgress, asyncLoad.progress);
            if (loadingScreenInstance != null)
            {
                loadingScreenInstance.UpdateLoadingProgress(currentProgress / 0.9f);
            }
            yield return null;
        }

        if (loadingScreenInstance != null)
            loadingScreenInstance.UpdateLoadingProgress(1f);

        // Đợi thêm vài giây để người dùng nhìn thấy thanh progress đầy đủ
        yield return new WaitForSeconds(1f);

        // Cho phép chuyển scene
        asyncLoad.allowSceneActivation = true;

        // Đợi cho đến khi scene được chuyển xong
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (loadingScreenInstance != null)
            loadingScreenInstance.HideLoadingScreen();
    }
}
