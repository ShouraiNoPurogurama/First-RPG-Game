using MainCharacter;
using Stats;
using UnityEngine;

namespace Enemies
{
    public class WindBossHammerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        private Animator _animator;
        private CircleCollider2D _circleCollider;
        private bool _attacked;
        private bool canMove = true;

        private Transform _player;
        private Vector3 _moveDirection; // The direction the hammer will travel
        private Vector3 _groundTarget;  // Where the hammer should stop

        [SerializeField] private int damage;
        [SerializeField] private float moveSpeed;
        private CharacterStats _myStats;

        public void SetupHammer(float speed, CharacterStats stats)
        {
            moveSpeed = speed;
            _myStats = stats;
        }

        private void Awake()
        {
            AttachCurrentPlayerIfNotExists();
            _animator = GetComponent<Animator>();
            _circleCollider = GetComponent<CircleCollider2D>();

            _moveDirection = (_player.position - transform.position).normalized;

            _groundTarget = GetGroundPosition();
        }

        private void Update()
        {
            if (!canMove) return;

            transform.position = Vector2.MoveTowards(transform.position, _groundTarget, moveSpeed * Time.deltaTime);

            if (!_attacked)
            {
                AttackTrigger();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _animator.SetBool("Landing", true);
                StuckInto(collision);
            }
        }

        private void StuckInto(Collider2D collision)
        {
            Debug.Log("Hammer Stuck into ground");

            _circleCollider.enabled = false;
            canMove = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.parent = collision.transform;
        }

        private void AttachCurrentPlayerIfNotExists()
        {
            if (!_player)
            {
                _player = PlayerManager.Instance.player.transform;
            }
        }

        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _circleCollider.radius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    _myStats.DoMagicalDamage(player.GetComponent<PlayerStats>());
                    _attacked = true;
                    player.FX.CreateHitFxThunder(player.transform);
                }
            }
        }

        private Vector3 GetGroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _moveDirection, 20f, LayerMask.GetMask("Ground"));
            return hit.collider != null ? hit.point : transform.position + _moveDirection * 20f;
        }
    
        public void SelfDestroy()
        {
            Destroy(gameObject);
        }
    }
}
