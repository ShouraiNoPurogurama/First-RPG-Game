using MainCharacter;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace WindMap
{
    public class FlyingWindController : MonoBehaviour
    {
        [Header("Trap Settings")]
        public GameObject windPrefab;

        public Transform spawnPoint;
        public float minThrowInterval = 1.5f;
        public float maxThrowInterval = 3.5f;
        public float throwForce = 10f;
        public float windLifetime = 30f;


        private Transform _player;

        private void Start()
        {
            _player = PlayerManager.Instance.player.transform;
            StartCoroutine(SummonWindRoutine());
        }

        private IEnumerator SummonWindRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minThrowInterval, maxThrowInterval));

                SummonWind();
            }
        }

        private void SummonWind()
        {
            if (windPrefab == null || spawnPoint == null) return;

            Debug.Log("Summoning wind");
            var yOffset = Random.Range(-4f, 4f);
            var spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y + yOffset, spawnPoint.position.z);
            GameObject wind = Instantiate(windPrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = wind.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 direction = (_player.position - spawnPoint.position).normalized;
                rb.linearVelocity = direction * throwForce;
            }

            Destroy(wind, windLifetime);
        }
        
    }
}