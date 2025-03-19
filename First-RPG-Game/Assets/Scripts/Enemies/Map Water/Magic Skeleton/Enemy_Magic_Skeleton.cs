using Skills.SkillControllers.Water_Map;
using UnityEngine;

namespace Enemies.Map_Water.Magic_Skeleton
{
    public class Enemy_Magic_Skeleton : Enemy
    {
        [Header("Magic Skeleton Info")]
        [SerializeField] public Vector2 jumpVelocity;
        [SerializeField] private GameObject waterBall;
        [SerializeField] private float arrowSpeed;
        public float JumpCooldown;
        public float safeDistance; // distance from player to start jumping
        [HideInInspector] public float lastTimeJumped;

        [Header("Additional Collison Check")]
        [SerializeField] private Transform groundBehindCheck;
        [SerializeField] private Vector2 groundBehindCheckSize;

        #region States

        public MagicSkeletonIdleState IdleState { get; private set; }
        public MagicSkeletonMoveState MoveState { get; private set; }
        public MagicSkeletonBattleState BattleState { get; private set; }
        public MagicSkeletonAttackState AttackState { get; private set; }
        public MagicSkeletonStunnedState StunnedState { get; private set; }
        public MagicSkeletonDeadState DeadState { get; private set; }
        public MagicSkeletonJumpState JumpState { get; private set; }
        #endregion
        protected override void Awake()
        {
            base.Awake();
            IdleState = new MagicSkeletonIdleState(this, StateMachine, "Idle", this);
            MoveState = new MagicSkeletonMoveState(this, StateMachine, "Move", this);
            BattleState = new MagicSkeletonBattleState(this, StateMachine, "Idle", this);
            AttackState = new MagicSkeletonAttackState(this, StateMachine, "Attack", this);
            StunnedState = new MagicSkeletonStunnedState(this, StateMachine, "Stunned", this);
            DeadState = new MagicSkeletonDeadState(this, StateMachine, "Idle", this);
            JumpState = new MagicSkeletonJumpState(this, StateMachine, "Jump", this);
            counterImage.SetActive(false);

        }
        protected override void Start()
        {
            base.Start();

            StateMachine.Initialize(IdleState);
        }
        protected override void Update()
        {
            base.Update();
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
            base.Die();
            StateMachine.ChangeState(DeadState);
        }

        public override void AnimationSpecialAttackTrigger()
        {
            GameObject newWaterBall = Instantiate(waterBall, transform.position, Quaternion.identity);
            if (FacingDir < 0)
            {
                newWaterBall.transform.Rotate(0, 180, 0);
            }
            newWaterBall.GetComponent<WaterBall_Controller>().SetupWaterBall(arrowSpeed * FacingDir, Stats);
        }

        public bool GroundBehind() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, 0, whatIsGround);
        public bool WallBehind() => Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDir, wallCheckDistance + 2, whatIsGround);

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize);
        }

    }
}
