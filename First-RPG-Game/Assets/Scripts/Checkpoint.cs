using MainCharacter;
using Manager_Controller;
using Save_and_Load;
using Stats;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activationStatus;
    private bool checkEnemiesBefore = true;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    [ContextMenu("Generate Checkpoint id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (AllEnemiesBeforeCheckpointDead())
            {
                activationStatus = true;
                anim.SetTrigger("active");
            }
        }
    }
    private bool AllEnemiesBeforeCheckpointDead()
    {
        Debug.Log("Check enemies");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy.transform.position.x < transform.position.x)
            {
                Debug.Log("Enemy found");
                EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                if (enemyStats != null && enemyStats.currentHp > 0)
                {
                    Debug.Log("Enemy alive");
                    Debug.Log("Enemy name: " + enemy.name);
                    Debug.Log("Enemy hp: " + enemyStats.currentHp);
                    return false;
                }
            }
        }
        return true;
    }
    public void ActivateCheckpoint()
    {
        if (activationStatus == false)
        {
            Debug.Log("Update");
            GameManager.Instance.UpdateCheckpointData(SaveManager.instance.GetGameData());
            string jsonData = JsonUtility.ToJson(GameManager.Instance.latestCheckpointData, true);
            Debug.Log("unity json" + jsonData);
            GameManager.Instance.latestCheckpointData.closeCheckpointId = id;
        }
        activationStatus = true;
        anim.SetTrigger("active");
    }
}
