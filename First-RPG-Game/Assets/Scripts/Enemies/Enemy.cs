using UnityEngine;

namespace Enemies
{
    public class Enemy : Entity
    {
        public LayerMask whatIsPlayer;

        [Header("Stunned info")]
        public float stunDuration;

        public Vector2 stunDirection;
        protected bool CanBeStunned;
        [SerializeField] protected GameObject counterImage;

        [Header("Move info")]
        public float moveSpeed;

        public float idleTime;
        public float battleTime;

        [Header("Attack info")]
        public float attackDistance;

        public float attackCooldown;
        [HideInInspector] public float lastTimeAttacked;

        protected EnemyStateMachine StateMachine { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            StateMachine = new EnemyStateMachine();
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

        public virtual void OpenCounterAttackWindow()
        {
            CanBeStunned = true;
            counterImage.SetActive(true);
        }

        public virtual void CloseCounterAttackWindow()
        {
            CanBeStunned = false;
            counterImage.SetActive(false);
        }

        public virtual bool IsCanBeStunned()
        {
            if (CanBeStunned)
            {
                CloseCounterAttackWindow();
                return true;
            }

            return false;
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