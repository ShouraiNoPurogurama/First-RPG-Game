using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class Enemy : Entity
    {
        public LayerMask whatIsPlayer;

        [Header("Stunned info")]
        public float stunDuration = 1f;

        public Vector2 stunDirection = new Vector2(5, 8);
        private bool _canBeStunned;
        [SerializeField] public GameObject counterImage;

        [Header("Move info")]
        public float moveSpeed = 2f;

        public float idleTime = 1f;
        public float battleTime = 3f;
        public float defaultMoveSpeed;

        [Header("Attack info")]
        public float attackDistance;
        public float agroDistance = 2;
        public float attackCooldown;
        private float _defaultAttackCooldown;
        [HideInInspector] public float lastTimeAttacked;

        public float minAttackCooldown = 1;
        public float maxAttackCooldown = 2;
        protected EnemyStateMachine StateMachine { get; private set; }

        public string LastAnimBoolName { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            StateMachine = new EnemyStateMachine();

            defaultMoveSpeed = moveSpeed;
            _defaultAttackCooldown = attackCooldown;
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            StateMachine.CurrentState.Update();
        }

        public virtual void FreezeTime(bool timeFrozen)
        {
            if (timeFrozen)
            {
                moveSpeed = 0;
                Animator.speed = 0;
            }
            else
            {
                moveSpeed = defaultMoveSpeed;
                Animator.speed = 1;
            }
        }

        protected virtual IEnumerator FreeTimerFor(float seconds)
        {
            FreezeTime(true);

            yield return new WaitForSeconds(seconds);

            FreezeTime(false);
        }

        #region Counter attack window

        public virtual void OpenCounterAttackWindow()
        {
            _canBeStunned = true;
            counterImage.SetActive(true);
        }

        public virtual void CloseCounterAttackWindow()
        {
            _canBeStunned = false;
            counterImage.SetActive(false);
        }

        #endregion

        public virtual bool IsCanBeStunned(bool forceStun)
        {
            if (forceStun)
            {
                return true;
            }

            if (_canBeStunned)
            {
                CloseCounterAttackWindow();
                return true;
            }

            return false;
        }

        public override void ReduceAttackSpeedBy(float slowPercentage, float slowDuration)
        {
            //Increase attack cooldown bc we dont have attack speed
            attackCooldown *= 1 + slowPercentage;

            Invoke("ReturnDefaultAttackSpeed", slowDuration);
        }

        protected override void ReturnDefaultAttackSpeed()
        {
            base.ReturnDefaultAttackSpeed();

            attackCooldown = _defaultAttackCooldown;
        }

        public override void SlowEntityBy(float slowPercentage, float slowDuration)
        {
            moveSpeed *= 1 - slowPercentage;
            Animator.speed *= 1 - slowPercentage;

            Invoke("ReturnDefaultSpeed", slowDuration);
        }

        protected override void ReturnDefaultSpeed()
        {
            base.ReturnDefaultSpeed();

            moveSpeed = defaultMoveSpeed;
        }

        public virtual void AssignLastAnimBoolName(string animName)
        {
            LastAnimBoolName = animName;
        }

        public virtual void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

        public virtual void AnimationSpecialAttackTrigger()
        {

        }

        public virtual void SecondaryAnimationSpecialAttackTrigger()
        {

        }

        public virtual void ThirdinaryAnimationSpecialAttackTrigger()
        {
            
        }

        public virtual void BusyMarker()
        {

        }

        public virtual RaycastHit2D IsPlayerDetected()
            => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, 50, whatIsPlayer);
        public bool IsPlayerInAttackRange()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(
                attackCheck.position,
                attackCheckRadius,
                whatIsPlayer
            );

            return hits.Length > 0;
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position,
                new Vector3(transform.position.x + attackDistance * FacingDir, transform.position.y));

        }
    }
}