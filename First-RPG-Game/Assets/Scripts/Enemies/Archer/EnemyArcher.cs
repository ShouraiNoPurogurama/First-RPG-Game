using Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.Archer
{
    public class EnemyArcher : Enemy
    {
        [Header("Soul specific info")]
        public Vector2 jumpVelocity;
        [SerializeField] public float runSpeed;
        [SerializeField] public float runDuration = 0.25f;

        [FormerlySerializedAs("arrow")] [SerializeField]
        private GameObject arrowPrefab;

        public float jumpCooldown;
        [HideInInspector] public float lastTimeJumped;
        public float safeDistance; //How close player should be to trigger jump on battle state
        public float meleeAttackDistance; //How close player should be to trigger melee attack on battle state
        [SerializeField] private float arrowSpeed;
        [SerializeField] private float arrowDamage;
        private CharacterStats _myStats;

        [SerializeField] private Transform groundBehindCheck;
        [SerializeField] private Vector2 groundBehindCheckSize;

        #region States

        public ArcherIdleState IdleState { get; private set; }
        public ArcherMoveState MoveState { get; private set; }
        public ArcherBattleState BattleState { get; private set; }
        public ArcherAttackState AttackState { get; private set; }
        public ArcherDeadState DeadState { get; private set; }
        public ArcherStunnedState StunnedState { get; private set; }
        public ArcherJumpState JumpState { get; private set; }
        public ArcherMeleeAttackState MeleeAttackState { get; private set; }
        
        public ArcherRunState RunState { get; private set; }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            IdleState = new ArcherIdleState(this, StateMachine, "Idle", this);
            MoveState = new ArcherMoveState(this, StateMachine, "Move", this);
            BattleState = new ArcherBattleState(this, StateMachine, "Idle", this);
            AttackState = new ArcherAttackState(this, StateMachine, "Shot", this);
            DeadState = new ArcherDeadState(this, StateMachine, "Dead", this);
            StunnedState = new ArcherStunnedState(this, StateMachine, "Stunned", this);
            JumpState = new ArcherJumpState(this, StateMachine, "Jump", this);
            MeleeAttackState = new ArcherMeleeAttackState(this, StateMachine, "MeleeAttack", this);
            RunState = new ArcherRunState(this, StateMachine, "Run", this);
        }

        protected override void Start()
        {
            base.Start();

            StateMachine.Initialize(IdleState);
        }

        public override bool IsCanBeStunned(bool forceStun)
        {
            if (base.IsCanBeStunned(forceStun))
            {
                StateMachine.ChangeState(StunnedState);
                return true;
            }

            return false;
        }

        public override void Flip()
        {
            if (IsBusy)
                return;

            base.Flip();

            StartCoroutine("BusyFor", .3f);
        }

        public override void Die()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 10);
            StateMachine.ChangeState(DeadState);
            base.Die();
        }

        public override void AnimationSpecialAttackTrigger()
        {
            base.AnimationSpecialAttackTrigger();
            GameObject newArrow = Instantiate(arrowPrefab, attackCheck.position, Quaternion.identity);

            newArrow.GetComponent<ArrowController>().SetupArrow(arrowSpeed * FacingDir, Stats);
        }

        public bool GroundBehindCheck() =>
            Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, whatIsGround);

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize);
        }
    }
}