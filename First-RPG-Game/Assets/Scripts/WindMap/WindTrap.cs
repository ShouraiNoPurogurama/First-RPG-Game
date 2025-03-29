using System;
using MainCharacter;
using Stats;
using UnityEngine;

namespace WindMap
{
    public class WindTrap : MonoBehaviour
    {
        [SerializeField] private int damage = 10;
        [SerializeField] private float pushForce = 5f;
        [SerializeField] private float pushDuration = 1f;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float xVelocity = -10f;

        private void Start()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }
        }

        private void Update()
        {
            rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Player player = collision.GetComponent<Player>();
                CharacterStats targetStats = collision.GetComponent<CharacterStats>();

                if (player != null && targetStats != null)
                {
                    targetStats.TakeDamage(damage);

                    Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                    if (playerRb != null)
                    {
                        Vector2 pushDirection = (player.transform.position - transform.position).normalized;
                        playerRb.linearVelocity = pushDirection * pushForce;
                        player.StartCoroutine(ResetVelocity(playerRb));
                    }
                }

                Destroy(gameObject);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
        }

        private System.Collections.IEnumerator ResetVelocity(Rigidbody2D playerRb)
        {
            yield return new WaitForSeconds(pushDuration);
            playerRb.linearVelocity = Vector2.zero;
        }
    }
}