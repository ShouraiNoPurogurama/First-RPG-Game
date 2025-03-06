using MainCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour, ISaveManager
{
    public static SceneController instance;
    private Transform player;

    [Header("Loading Screen Reference")]
    public LoadingScreen loadingScreenInstance;
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closetCheckpointId;

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

    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            RestartScene();
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
        // Stop activate loading
        asyncLoad.allowSceneActivation = false;

        float simulatedProgress = 0f;
        float speed = 0.3f;

        while (simulatedProgress < 0.9f)
        {
            // Increase the progress bar
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

        yield return new WaitForSeconds(1f);

        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (loadingScreenInstance != null)
            loadingScreenInstance.HideLoadingScreen();
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                {
                    checkpoint.ActivateCheckpoint();
                }
            }
        }
        closetCheckpointId = _data.closeCheckpointId;
        Invoke("PlacePlayerAtClosetCheckpoint", .1f);
    }

    private void PlacePlayerAtClosetCheckpoint()
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (closetCheckpointId == checkpoint.id)
            {
                PlayerManager.Instance.player.transform.position = checkpoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.closeCheckpointId = FindClosestCheckpoint().id;
        _data.checkpoints.Clear();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.Instance.player.transform.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }
    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(scene.name);
    }
}
