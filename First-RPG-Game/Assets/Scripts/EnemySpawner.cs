using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab Enemy Skeleton")]
    public GameObject enemyPrefab;

    [Header("Vị trí sinh Enemy (nếu để trống, sẽ dùng vị trí Spawner)")]
    public Transform spawnPoint;

    private bool hasSpawned = false;

    private void Start()
    {
        if (spawnPoint == null)
        {
            spawnPoint = this.transform;
            Debug.LogWarning("⚠️ spawnPoint bị để trống, tự động gán thành vị trí EnemySpawner.");
        }

        // Đăng ký sự kiện reset khi load scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset hasSpawned khi vào scene mới
        hasSpawned = false;
        Debug.Log("🔄 Scene mới: Reset trạng thái Spawner.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasSpawned && collision.CompareTag("Player"))
        {
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("💀 EnemySkeleton đã được sinh ra!");
            hasSpawned = true;
        }
        else
        {
            Debug.Log("Error: Không thể sinh Enemy.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
