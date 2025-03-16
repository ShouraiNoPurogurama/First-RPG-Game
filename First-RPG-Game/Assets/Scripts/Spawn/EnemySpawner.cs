using System.Collections.Generic;
using UnityEngine;

namespace Spawn
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("List spawn enemy")]
        public List<SpawnPointData> spawnPointGroups;
        [Header("Reference to Player")]
        public Transform player;
        public void SpawnAllEnemies()
        {
            Debug.LogWarning("SpawnPoint in");
            foreach (SpawnPointData group in spawnPointGroups)
            {
                if (group.spawnPoint == null)
                {
                    Debug.LogWarning("SpawnPoint trống, bỏ qua nhóm này.");
                    continue;
                }

                float playerX = player.position.x;
                float spawnX = group.spawnPoint.position.x;

                if (spawnX < playerX)
                {
                    continue;
                }

                if (group.enemyList != null && group.enemyList.Count > 0)
                {
                    foreach (EnemyData enemyData in group.enemyList)
                    {
                        Instantiate(enemyData.enemyPrefab, group.spawnPoint.position, Quaternion.identity);
                    }
                }
            }
        }
    }
}
