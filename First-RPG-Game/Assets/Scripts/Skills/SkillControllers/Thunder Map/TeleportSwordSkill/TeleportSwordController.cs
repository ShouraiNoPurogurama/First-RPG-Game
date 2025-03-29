using Enemies;
using MainCharacter;
using Stats;
using UnityEngine;

namespace Skills.SkillControllers.Thunder_Map.TeleportSwordSkill
{
    public class TeleportSwordController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Player _player;
        private bool _hasHitWall;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Setup(Vector2 velocity, float gravityScale, Player player)
        {
            _player = player;
            _rb.linearVelocity = velocity;
            _rb.gravityScale = gravityScale;
            _hasHitWall = false;

            Destroy(gameObject, 5f); // Hủy kiếm sau 5 giây nếu không va chạm
        }

        private void Update()
        {
            transform.right = _rb.linearVelocity; // Xoay kiếm theo hướng bay
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (_hasHitWall) return;

        //    // Kiểm tra va chạm với tường (giả sử tường có tag "Wall")
        //    if (collision.CompareTag("Ground"))
        //    {
        //        _hasHitWall = true;
        //        TeleportPlayer();
        //        Destroy(gameObject); // Hủy kiếm sau khi dịch chuyển
        //    }
        //    // Kiểm tra va chạm với kẻ địch
        //    else if (collision.GetComponent<Enemy>() != null)
        //    {
        //        Enemy enemy = collision.GetComponent<Enemy>();
        //        _player.Stats.DoDamage(enemy.GetComponent<EnemyStats>()); // Gây sát thương
        //        Destroy(gameObject); // Hủy kiếm khi trúng kẻ địch
        //    }
        //}

        private void TeleportPlayer()
        {
            _player.transform.position = transform.position; // Dịch chuyển player đến vị trí kiếm
        }
    }
}