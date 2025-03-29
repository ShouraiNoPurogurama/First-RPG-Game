using UnityEngine;
using System.Collections;

namespace Enemies.Enemies_Fire
{
    public class RandomEnemy : MonoBehaviour
    {
        public GameObject enemyPre;
        public int spawnCount = 3; 
        public float spawnInterval = 1f;

        private void Start()
        {
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnSkelekon();
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void SpawnSkelekon()
        {
            Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            Vector3 spawnPosition = transform.position + randomOffset;
            Instantiate(enemyPre, spawnPosition, Quaternion.identity);
        }
    }
}
