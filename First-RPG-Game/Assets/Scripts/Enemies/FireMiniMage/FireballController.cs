using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies.FireMiniMage
{
    public class FireballController : MonoBehaviour
    {
        [SerializeField] private string targetLayerName = "Player";
        [SerializeField] private int damage;
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float maxDistance = 20f;

        private Vector2 startPosition;
        private bool flipped = false;
        private CharacterStats _myStats;
        void Update()
        {

        }
        public void SetupFireball(float fireballSpeed, CharacterStats stats)
        {
            speed = fireballSpeed;
            _myStats = stats;
            startPosition = transform.position; // start
            rb.linearVelocity = new Vector2(speed, 0); // start move
        }

        private void FixedUpdate()
        {
            if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                if (player.StateMachine.CurrentState is PlayerCounterAttackState)
                {
                    Destroy(gameObject);  
                    return;
                }
                Debug.Log("Cham roi ne");
                player.Stats.TakeDamage(30, Color.yellow);
                //rb.linearVelocity = Vector2.zero;
                Destroy(gameObject);
            }
        }

        public void FlipFireball()
        {
            if (flipped) return;

            flipped = true;
            targetLayerName = "Enemy";
            speed = -speed;
            transform.Rotate(0, 180, 0);
            rb.linearVelocity = new Vector2(speed, 0);
        }
    }
}
