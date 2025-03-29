using MainCharacter;
using Save_and_Load;
using Spawn;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour, ISaveManager
{
    public static SceneController instance;
    private Transform player;
    private EnemySpawner enemySpawner;

    [Header("Loading Screen Reference")]
    public LoadingScreen loadingScreenInstance;
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closetCheckpointId;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            RestartScene();
        if (Input.GetKeyDown(KeyCode.O))
            LoadMenuScene();
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
    /// <summary>
    /// Load checkpoints from GameData
    /// </summary>
    /// <param name="_data"></param>
    public void LoadData(GameData _data)
    {
        Checkpoint[] allCheckpoints = FindObjectsOfType<Checkpoint>();

        List<Checkpoint> toActivate = new List<Checkpoint>();

        foreach (var pair in _data.checkpoints)
        {
            string checkpointId = pair.Key;
            bool isActive = pair.Value;

            Checkpoint matchedCheckpoint = allCheckpoints
                .FirstOrDefault(c => c.id == checkpointId);

            if (matchedCheckpoint != null && isActive)
            {
                toActivate.Add(matchedCheckpoint);
            }
        }

        foreach (Checkpoint cp in toActivate)
        {
            cp.ActivateCheckpoint();
        }

        // Lưu lại id của checkpoint gần nhất
        closetCheckpointId = _data.closeCheckpointId;

        // Đợi cho đến khi PlayerManager và tất cả Checkpoint đã được khởi tạo xong
        StartCoroutine(PlacePlayerAfterReady());
    }

    private IEnumerator PlacePlayerAfterReady()
    {
        yield return new WaitUntil(() => PlayerManager.Instance != null && PlayerManager.Instance.player != null);
        yield return new WaitUntil(() => FindObjectsOfType<Checkpoint>().Length > 0);
        PlacePlayerAtClosetCheckpoint();
    }

    /// <summary>
    /// Place player at the closet checkpoint
    /// </summary>
    private void PlacePlayerAtClosetCheckpoint()
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (closetCheckpointId == checkpoint.id)
            {
                PlayerManager.Instance.player.transform.position = checkpoint.transform.position;
            }
        }
        enemySpawner.SpawnAllEnemies();
    }
    /// <summary>
    /// Load the main menu scene
    /// </summary>
    public void LoadMenuScene()
    {
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }
    public void SaveData(ref GameData _data)
    {
        //_data.closeCheckpointId = FindClosestCheckpoint().id;
        Checkpoint cp = FindClosestCheckpoint();
        if (cp != null)
        {
            Debug.Log("Closest checkpoint: " + cp.id);
            _data.closeCheckpointId = cp.id;
        }
        else
        {
            _data.closeCheckpointId = "";
            Debug.LogWarning("Không tìm thấy checkpoint nào để lưu.");
        }
        _data.checkpoints.Clear();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != "MainMenu")
        {
            _data.screenName = currentScene.name;
        }
    }
    /// <summary>
    /// Find the closest checkpoint to the player
    /// </summary>
    /// <returns></returns>
    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = -1;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.Instance.player.transform.position, checkpoint.transform.position);
            Debug.Log("Distance to " + checkpoint.id + ": " + distanceToCheckpoint);
            if (distanceToCheckpoint > closestDistance && checkpoint.activationStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }
    /// <summary>
    /// Restart the current scene
    /// </summary>
    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    /// <summary>
    /// Get all checkpoints in the scene
    /// </summary>
    /// <returns></returns>
    public SerializableDictionary<string, bool> GetCheckpoints()
    {
        SerializableDictionary<string, bool> checkpointsData = new SerializableDictionary<string, bool>();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            checkpointsData.Add(checkpoint.id, checkpoint.activationStatus);
        }
        return checkpointsData;
    }
    public void PauseGame(bool _paused)
    {
        if (_paused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}
