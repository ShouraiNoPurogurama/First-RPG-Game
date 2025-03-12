using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class Enemy : Entity
    {
        public LayerMask whatIsPlayer;

        [Header("Stunned info")]
        public float stunDuration;

        public Vector2 stunDirection;
        private bool _canBeStunned;
        [SerializeField] protected GameObject counterImage;

        [Header("Move info")]
        public float moveSpeed;

        public float idleTime;
        public float battleTime;
        private float _defaultMoveSpeed;

        [Header("Attack info")]
        public float attackDistance;

        public float attackCooldown;
        [HideInInspector] public float lastTimeAttacked;
        
        protected EnemyStateMachine StateMachine { get; private set; }
        
        public string LastAnimBoolName { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            StateMachine = new EnemyStateMachine();

            _defaultMoveSpeed = moveSpeed;
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
                moveSpeed = _defaultMoveSpeed;
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

        public override void SlowEntityBy(float slowPercentage, float slowDuration)
        {
            moveSpeed *= 1 - slowPercentage;
            Animator.speed *= 1 - slowPercentage;
            
            Invoke("ReturnDefaultSpeed", slowDuration);
        }

        protected override void ReturnDefaultSpeed()
        {
            base.ReturnDefaultSpeed();
            
            moveSpeed = _defaultMoveSpeed;
        }

        public virtual void AssignLastAnimBoolName(string animName)
        {
            LastAnimBoolName = animName;
        }
        
        public virtual void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

        public virtual RaycastHit2D IsPlayerDetected()
            => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, 50, whatIsPlayer);


        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position,
                new Vector3(transform.position.x + attackDistance * FacingDir, transform.position.y));
        }
    }
}