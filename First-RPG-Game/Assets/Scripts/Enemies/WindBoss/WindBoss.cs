using Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.WindBoss
{
    public class WindBoss : Enemy
    {
        [Header("WindBoss specific info")]
        public Vector2 jumpVelocity;
        [SerializeField] public float runSpeed;
        [SerializeField] public float runDuration = 0.25f;
        
        public float jumpCooldown;
        [HideInInspector] public float lastTimeJumped;
        public float meleeAttackDistance; //How close player should be to trigger melee attack on battle state

        public float triggerLeapDistance; //How close player should be to trigger jump on battle state
        [SerializeField] private GameObject hammerPrefab;
        [SerializeField] private float hammerSpeed;
        [SerializeField] private float hammerDamage;
        [SerializeField] public float leapCoolDown;
        [HideInInspector] public float lastTimeLeaped;
        
        private CharacterStats _myStats;

        [SerializeField] private Transform groundBehindCheck;
        [SerializeField] private Vector2 groundBehindCheckSize;

        #region States

        public WindBossIdleState IdleState { get; private set; }
        public WindBossMoveState MoveState { get; private set; }
        public WindBossBattleState BattleState { get; private set; }
        public WindBossLeapAttackState LeapAttackState { get; private set; }
        public WindBossDeadState DeadState { get; private set; }
        public WindBossStunnedState StunnedState { get; private set; }
        public WindBossJumpState JumpState { get; private set; }
        public WindBossMeleeAttackState MeleeAttackState { get; private set; }
        public WindBossRunState RunState { get; private set; }
        public WindBossLeapState LeapState { get; private set; }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            IdleState = new WindBossIdleState(this, StateMachine, "Idle", this);
            MoveState = new WindBossMoveState(this, StateMachine, "Move", this);
            BattleState = new WindBossBattleState(this, StateMachine, "Move", this);
            LeapAttackState = new WindBossLeapAttackState(this, StateMachine, "LeapAttack", this);
            DeadState = new WindBossDeadState(this, StateMachine, "Dead", this);
            StunnedState = new WindBossStunnedState(this, StateMachine, "Stunned", this);
            JumpState = new WindBossJumpState(this, StateMachine, "Jump", this);
            MeleeAttackState = new WindBossMeleeAttackState(this, StateMachine, "Attack", this);
            // RunState = new WindBossRunState(this, StateMachine, "Run", this);
            LeapState = new WindBossLeapState(this, StateMachine, "Leap", this);
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
            GameObject newHammer = Instantiate(hammerPrefab, attackCheck.position, Quaternion.identity);
            newHammer.GetComponent<WindBossHammerController>().SetupHammer(hammerSpeed, Stats);
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