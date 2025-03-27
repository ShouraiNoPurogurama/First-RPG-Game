using MainCharacter;
using UnityEngine;
using UnityEngine.Serialization;

namespace WindMap
{
    public class FlyingWindController : MonoBehaviour
    {
        [FormerlySerializedAs("rockPrefab")]
        [Header("Trap Settings")]
        public GameObject windPrefab;
        public Transform spawnPoint;
        public float minThrowInterval = 1.5f;
        public float maxThrowInterval = 3.5f;
        public float throwForce = 10f;
        public float rockLifetime = 5f;

        [Header("Detection Area")]
        public Vector2 detectionAreaSize = new Vector2(10f, 5f);
        
        [SerializeField] public LayerMask whatIsPlayer; 

        private Transform _player;
        private bool _playerInRegion;

        private void Start()
        {
            _player = PlayerManager.Instance.player.transform;
            StartCoroutine(SummonWindRoutine());
        }

        private void Update()
        {
            _playerInRegion = Physics2D.OverlapBox(transform.position, detectionAreaSize, 0f, whatIsPlayer);
            if (_playerInRegion)
            {
                Debug.Log(_playerInRegion);
            }
        }

        private System.Collections.IEnumerator SummonWindRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minThrowInterval, maxThrowInterval));

                if (_playerInRegion)
                {
                    SummonWind();
                }
            }
        }

        private void SummonWind()
        {
            if (windPrefab == null || spawnPoint == null) return;

            GameObject wind = Instantiate(windPrefab, spawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = wind.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 direction = (_player.position - spawnPoint.position).normalized;
                rb.linearVelocity = direction * throwForce;
            }

            Destroy(wind, rockLifetime);
        }

        // Debug visualization in Scene View
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawCube(transform.position, detectionAreaSize);
        }
    }
}
