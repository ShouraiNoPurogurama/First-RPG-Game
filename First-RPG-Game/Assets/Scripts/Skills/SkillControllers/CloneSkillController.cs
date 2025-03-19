using Enemies;
using MainCharacter;
using Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Skills.SkillControllers
{
    public class CloneSkillController : MonoBehaviour
    {
        private Player _player;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;

        [SerializeField] private float colorLoosingSpeed;
        private float _cloneTimer;
        private int _cloneFacingDir = 1;

        [SerializeField] private Transform attackCheck;
        [SerializeField] private float attackCheckRadius = .8f;
        private Transform _closestEnemy;

        private bool _canDuplicateClone;
        private float _chanceToDuplicate;

        //Fix sprite position
        private readonly Vector3 _defaultYOffset = new Vector3(0, -0.3f);

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _cloneTimer -= Time.deltaTime;

            if (_cloneTimer < .4)
            {
                _spriteRenderer.color = new Color(1, 1, 1, _spriteRenderer.color.a - Time.deltaTime * colorLoosingSpeed);
                if (_spriteRenderer.color.a <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack, Vector3 offset,
            Transform closestEnemy, bool canDuplicate, float chanceToDuplicate, int facingDir, Player player)
        {
            if (canAttack)
            {
                _animator.SetInteger("AttackNumber", Random.Range(1, 3));
            }

            _player = player;

            transform.position = newTransform.position + offset + _defaultYOffset;

            _closestEnemy = closestEnemy;

            _canDuplicateClone = canDuplicate;

            _chanceToDuplicate = chanceToDuplicate;

            _cloneFacingDir = facingDir;

            FaceClosestTarget();

            _cloneTimer = cloneDuration;
        }

        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

            foreach (var hit in colliders)
            {
                var enemy = hit.GetComponent<Enemy>();
                if (enemy is not null)
                {
                    hit.GetComponent<Entity>().SetupKnockBackDir(transform);
                    _player.Stats.DoDamage(enemy.GetComponent<EnemyStats>());
                    enemy.FX.CreateHitFx(enemy.transform, false);

                    if (_canDuplicateClone)
                    {
                        if (Random.Range(0, 100) < _chanceToDuplicate)
                        {
                            SkillManager.Instance.Clone.CreateClone(enemy.transform,
                                new Vector3(1.2f * _cloneFacingDir, 0));
                        }
                    }
                }
            }
        }

        private void AnimationTrigger()
        {
            _cloneTimer -= -.1f;
        }

        private void FaceClosestTarget()
        {
            if (_closestEnemy)
            {
                if (transform.position.x > _closestEnemy.position.x)
                {
                    _cloneFacingDir = -1;
                    transform.Rotate(0, 180, 0);
                }
            }
        }
    }
}