using System;
using Enemies;
using UnityEngine;

namespace Skills.SkillControllers
{
    public class CrystalSkillController : MonoBehaviour
    {
        private Animator Animator => GetComponent<Animator>();
        private CircleCollider2D CircleCollider => GetComponent<CircleCollider2D>();

        private float _crystalExistTimer;
        private bool _canExplode;
        private bool _canMove;
        private float _moveSpeed;

        private bool _canGrow;
        [SerializeField] private float growSpeed;

        private Transform _closestTarget;
        
        public void SetUpCrystal(float crystalDuration, bool canExplode, bool canMove, float moveSpeed, Transform closestTarget)
        {
            _crystalExistTimer = crystalDuration;
            _canExplode = canExplode;
            _canMove = canMove;
            _moveSpeed = moveSpeed;
            _closestTarget = closestTarget;
        }

        private void Update()
        {
            _crystalExistTimer -= Time.deltaTime;

            if (_crystalExistTimer < 0)
            {
                FinishCrystal();
            }

            if (_canMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, _closestTarget.position, growSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, _closestTarget.position) < 1)
                {
                    FinishCrystal();
                    _canMove = false;
                }
            }
            
            if (_canGrow)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
            }
        }

        public void FinishCrystal()
        {
            if (_canExplode)
            {
                _canGrow = true;
                Animator.SetTrigger("Explode");
            }
            else
            {
                SelfDestroy();
            }
        }

        public void SelfDestroy() => Destroy(gameObject);

        private void AnimationExplodeEvent()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, CircleCollider.radius);

            foreach (var hit in colliders)
            {
                hit.GetComponent<Enemy>()?.Damage();
            }
        }
    }
}