using MainCharacter;
using Stats;
using UnityEngine;

namespace Skills
{
    public class Arrow_Controller : MonoBehaviour
    {
        [SerializeField] private string targetLayerName = "Player";
        [SerializeField] public int damage;
        [SerializeField] private float xVelocity;
        [SerializeField] private float yVelocity;
        [SerializeField] private Rigidbody2D rb;
        private float timeDefault;

        private void Start()
        {
            timeDefault = 4f; // Set the default time to 3 seconds
        }

        private void Update()
        {
            if (this == null || gameObject == null) return;

            timeDefault -= Time.deltaTime;
            if (timeDefault <= 0)
            {
                Destroy(gameObject);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>())
            {
                CharacterStats targetStats = collision.GetComponent<CharacterStats>();
                if (targetStats != null)
                {
                    targetStats.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
        }
        public void SetVelocity(Vector2 velocity)
        {
            if (this == null || gameObject == null || rb == null) return; // Tránh lỗi khi object bị hủy
            rb.linearVelocity = velocity;
        }

    
    }
}
