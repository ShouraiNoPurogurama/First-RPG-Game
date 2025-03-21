using MainCharacter;
using Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.Frogger
{
    public class EnemyFrogger : Enemy
    {
        [FormerlySerializedAs("spitDamageScaleByBaseDmg")]
        [Header("Fogger specific info")]
        [SerializeField] private float spitDamageScale;

        public float spitDistance;

        [SerializeField] public float regenScaleByHp;
        [SerializeField] public float sizeScaleAfterRegen;
        [SerializeField] public float jumpDistance;
        [SerializeField] public Vector2 jumpVelocity;
        [SerializeField] public float regenDuration;
        [SerializeField] public float regenCooldown;
        [HideInInspector] public float lastTimeRegen;
        [SerializeField] public float spitCooldown;
        [HideInInspector] public float lastTimeSpit;

        private CharacterStats _myStats;

        [SerializeField] private Transform groundBehindCheck;
        [SerializeField] private Vector2 groundBehindCheckSize;

        #region States

        public FroggerIdleState IdleState { get; private set; }
        public FroggerMoveState MoveState { get; private set; }
        public FroggerBattleState BattleState { get; private set; }
        public FroggerTongueAttackState TongueAttackState { get; private set; }
        public FroggerSpitAttackState SpitAttackState { get; private set; }
        public FroggerDeadState DeadState { get; private set; }
        public FroggerStunnedState StunnedState { get; private set; }
        public FroggerRegenState RegenState { get; private set; }
        public FroggerJumpState JumpState { get; private set; }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            IdleState = new FroggerIdleState(this, StateMachine, "Idle", this);
            MoveState = new FroggerMoveState(this, StateMachine, "Move", this);
            BattleState = new FroggerBattleState(this, StateMachine, "Move", this);
            TongueAttackState = new FroggerTongueAttackState(this, StateMachine, "TongueAttack", this);
            DeadState = new FroggerDeadState(this, StateMachine, "Dead", this);
            StunnedState = new FroggerStunnedState(this, StateMachine, "Stunned", this);
            RegenState = new FroggerRegenState(this, StateMachine, "Regen", this);
            SpitAttackState = new FroggerSpitAttackState(this, StateMachine, "SpitAttack", this);
            JumpState = new FroggerJumpState(this, StateMachine, "Jump", this);
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
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player)
                {
                    Debug.Log("Hit player with spit");
                    Stats.DoMagicalDamage(player.GetComponent<PlayerStats>(), spitDamageScale);
                }
            }
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