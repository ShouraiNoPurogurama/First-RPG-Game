using Enemies;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Skills.SkillControllers
{
    public class CloneSkillController : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;

        [SerializeField] private float colorLoosingSpeed;
        private float _cloneTimer;

        [SerializeField] private Transform attackCheck;
        [SerializeField] private float attackCheckRadius = .8f;
        private Transform _closestEnemy;

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

        public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack, Vector3 offset)
        {
            if (canAttack)
            {
                _animator.SetInteger("AttackNumber", Random.Range(1, 3));
            }
            
            transform.position = newTransform.position + offset;
            
            FaceClosestTarget();

            _cloneTimer = cloneDuration;
        }

        private void AttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

            foreach (var hit in colliders)
            {
                hit.GetComponent<Enemy>()?.Damage();
            }
        }

        private void AnimationTrigger()
        {
            _cloneTimer -= -.1f;
        }

        private void FaceClosestTarget()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

            float closestDistance = math.INFINITY;

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() is not null)
                {
                    float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        _closestEnemy = hit.transform;
                    }
                }
            }

            if (_closestEnemy is not null)
            {
                if (transform.position.x > _closestEnemy.position.x)
                {
                    transform.Rotate(0, 180, 0);
                }
            }
        }
    }
}