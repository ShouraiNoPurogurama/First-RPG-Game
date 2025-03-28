using MainCharacter;
using Stats;
using UnityEngine;

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

        [Header("Leap attack info")]
        public float triggerLeapDistance; //How close player should be to trigger jump on battle state

        [SerializeField] private GameObject hammerPrefab;
        [SerializeField] private float hammerSpeed;
        [SerializeField] private float hammerDamage;
        [SerializeField] public float leapCoolDown;
        [HideInInspector] public float lastTimeLeaped;

        [Header("Spin attack info")]
        [SerializeField] private float spinDistance;

        [SerializeField] public float spinDamageScale;
        [SerializeField] public float spinCoolDown;
        [HideInInspector] public float lastTimeSpin;
        [SerializeField] public float spinDuration;


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
        public WindBossLeapState LeapState { get; private set; }
        public WindBossEnterSpinAttackState EnterSpinAttackState { get; private set; }
        public WindBossSpinAttackState SpinAttackState { get; private set; }

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
            LeapState = new WindBossLeapState(this, StateMachine, "Leap", this);
            EnterSpinAttackState = new WindBossEnterSpinAttackState(this, StateMachine, "EnterSpinAttack", this);
            SpinAttackState = new WindBossSpinAttackState(this, StateMachine, "SpinAttack", this);
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
            float xOffset = FacingDir < 0 ? -2.5f : 2.5f;

            Vector3 spawnPosition = new Vector3(attackCheck.position.x + xOffset, attackCheck.position.y - 2f, 0f);

            GameObject newHammer = Instantiate(hammerPrefab, spawnPosition, Quaternion.identity);
            newHammer.GetComponent<WindBossHammerController>().SetupHammer(hammerSpeed, Stats);
        }

        public override void SecondaryAnimationSpecialAttackTrigger()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    Stats.DoMagicalDamage(player.GetComponent<PlayerStats>());
                    Debug.Log("Freeze boss movement");
                    SetZeroVelocity();
                }
            }
        }

        public override void BusyMarker()
        {
            StartCoroutine("BusyFor", .5f);
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