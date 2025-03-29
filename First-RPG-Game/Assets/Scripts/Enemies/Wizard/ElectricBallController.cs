using UnityEngine;
using Stats;

namespace Enemies.Wizard
{
    public class ElectricBallController : MonoBehaviour
    {
        private float _speed;
        private EnemyWizard _wizard; // Tham chiếu đến Wizard để lấy Stats

        public void SetupElectricBall(float speed, EnemyWizard wizard)
        {
            _speed = speed;
            _wizard = wizard;
            Destroy(gameObject, 5f); // Tự hủy sau 5 giây nếu không va chạm
        }

        private void Update()
        {
            transform.Translate(_speed * Time.deltaTime, 0, 0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                CharacterStats playerStats = collision.GetComponent<CharacterStats>();
                if (playerStats != null && _wizard != null)
                {
                    // Gây sát thương ma thuật dựa trên lightingDamage của Wizard
                    _wizard.Stats.DoMagicalDamage(playerStats);
                }
                Destroy(gameObject);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
        }
    }
}