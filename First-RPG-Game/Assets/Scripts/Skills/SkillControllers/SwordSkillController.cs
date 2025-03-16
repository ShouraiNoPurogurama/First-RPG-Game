using System.Collections.Generic;
using Enemies;
using MainCharacter;
using Stats;
using UnityEngine;

namespace Skills.SkillControllers
{
    public class SwordSkillController : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody2D _rb;
        private CircleCollider2D _circleCollider2D;
        private Player _player;
        private bool _canRotate = true;
        private bool _isReturning;
        private float _returnSpeed = 15;

        private float _freezeTimeDuration;

        [Header("Pierce info")]
        private float _pierceAmount;

        [Header("Bounce info")]
        private bool _isBouncing;

        private int _amountOfBounce;
        private int _targetIndex;
        private List<Transform> _enemyTargets;
        private float _bounceSpeed;

        [Header("Spin info")]
        private float _maxTravelDistance;

        private float _spinDuration;
        private float _spinTimer;
        private bool _wasStopped;
        private bool _isSpinning;

        private float _hitTimer;
        private float _hitCooldown;
        private float _spinDirection;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
        }

        public void SetupSword(Vector2 dir, float gravityScale, Player player, float freezeTimeDuration, float returnSpeed)
        {
            _player = player;
            _returnSpeed = returnSpeed;

            _rb.linearVelocity = dir;
            _rb.gravityScale = gravityScale;
            _freezeTimeDuration = freezeTimeDuration;

            if (_pierceAmount <= 0)
            {
                _animator.SetBool("Rotation", true);
            }

            //Returns the min value if the given value is less than the min value.
            //Returns the max value if the given value is greater than the max value.
            _spinDirection = Mathf.Clamp(_rb.linearVelocity.x, -1, 1);

            Invoke("DestroyMe", 7);
        }

        public void SetupBounce(bool isBouncing, int amountOfBounce, float bounceSpeed)
        {
            _bounceSpeed = bounceSpeed;
            _isBouncing = isBouncing;
            _amountOfBounce = amountOfBounce;
            _enemyTargets = new List<Transform>();
        }

        public void SetupPierce(int pierceAmount)
        {
            _pierceAmount = pierceAmount;
        }

        public void SetupSpin(bool isSpinning, float maxTravelDistance, float spinDuration, float hitTimer, float hitCooldown)
        {
            _isSpinning = isSpinning;
            _maxTravelDistance = maxTravelDistance;
            _spinDuration = spinDuration;
            _hitTimer = hitTimer;
            _hitCooldown = hitCooldown;
        }

        public void ReturnSword()
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.parent = null;
            _isReturning = true;
        }

        private void Update()
        {
            if (_canRotate)
            {
                transform.right = _rb.linearVelocity;
            }

            BouncingLogic();

            SpinningLogic();

            ReturningLogic();
        }

        private void BouncingLogic()
        {
            if (!_isBouncing || _enemyTargets.Count <= 0) return;

            Transform targetTransform = _enemyTargets[_targetIndex];
            Enemy targetEnemy = targetTransform.GetComponent<Enemy>();

            transform.position =
                Vector2.MoveTowards(transform.position, targetTransform.position, _bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetTransform.position) < 0.1f)
            {
                SwordSkillDamage(targetEnemy);

                _targetIndex++;
                _amountOfBounce--;

                if (_amountOfBounce <= 0)
                {
                    _isBouncing = false;
                    _isReturning = true;
                }

                if (_targetIndex >= _enemyTargets.Count)
                {
                    _targetIndex = 0;
                }
            }
        }

        private void SwordSkillDamage(Enemy targetEnemy)
        {
            _player.Stats.DoDamage(targetEnemy.GetComponent<EnemyStats>());
            targetEnemy.FX.CreateHitFx(targetEnemy.transform, false);
            targetEnemy.StartCoroutine("FreeTimerFor", 1f);
        }

        private void SpinningLogic()
        {
            if (_isSpinning)
            {
                transform.localScale = new Vector3(1.5f, 1.5f);
                _animator.speed = 1.5f;
                
                if (Vector2.Distance(_player.transform.position, transform.position) > _maxTravelDistance && !_wasStopped)
                {
                    StopWhenSpinning();
                }

                if (_wasStopped)
                {
                    _spinTimer -= Time.deltaTime;

                    transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(transform.position.x + _spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                    if (_spinTimer < 0)
                    {
                        _isSpinning = false;
                        _isReturning = true;
                    }

                    _hitTimer -= Time.deltaTime;

                    if (_hitTimer < 0)
                    {
                        _hitTimer = _hitCooldown;
                        
                        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                        foreach (var hit in colliders)
                        {
                            var enemy = hit.GetComponent<Enemy>();
                            if (enemy is not null)
                            {
                                SwordSkillDamage(enemy);
                            }
                        }
                    }
                }
            }
        }

        private void ReturningLogic()
        {
            if (_isReturning)
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, _player.transform.position,
                        1.5f * _returnSpeed *
                        Time.deltaTime); //maxDistanceDelta: maximum distance the object can move in one frame (one Update() cycle)

                if (Vector2.Distance(transform.position, _player.transform.position) < 1)
                {
                    _player.CatchSword();
                }
            }
        }

        private void StopWhenSpinning()
        {
            _wasStopped = true;
            _rb.constraints = RigidbodyConstraints2D.FreezePosition;
            _spinTimer = _spinDuration;
        }


        //Disable rotation, prevent Rigidbody-driven movement and assign sword to enemy's collider2D as a children
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isReturning) //Prevent stuck into all colliders
            {
                return;
            }

            var enemy = collision.GetComponent<Enemy>();

            if (enemy is not null)
            {
                SwordSkillDamage(enemy);

                SetupTargetForBouncing();
            }

            StuckInto(collision);
        }

        private void SetupTargetForBouncing()
        {
            if (_isBouncing && _enemyTargets.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() is not null)
                    {
                        _enemyTargets.Add(hit.transform);
                    }
                }
            }
        }

        private void StuckInto(Collider2D collision)
        {
            if (_isSpinning)
            {
                StopWhenSpinning();
                return;
            }

            if (_pierceAmount > 0 && collision.GetComponent<Enemy>() is not null)
            {
                _pierceAmount--;
                return;
            }

            _canRotate = false;
            _circleCollider2D.enabled = false;

            _rb.bodyType =
                RigidbodyType2D
                    .Kinematic; //the Kinematic Rigidbody 2D is not moved by the physics system when contacting another Rigidbody 2D. Instead, it behaves like an immovable object with infinite mass
            _rb.constraints =
                RigidbodyConstraints2D
                    .FreezeAll; //the sword still moves with the parent object because of Transform Hierarchy Propagation

            if (_isBouncing && _enemyTargets.Count > 0)
                return;

            _animator.SetBool("Rotation", false);
            transform.parent = collision.transform;
        }

        private void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}