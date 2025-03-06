using Enemies;
using UnityEngine;

public class Enemy_Magic_Skeleton : Enemy
{
    [Header("Magic Skeleton Info")]
    [SerializeField] public Vector2 jumpVelocity;
    [SerializeField] private GameObject waterBall;
    public float JumpCooldown;
    public float safeDistance; // distance from player to start jumping
    [HideInInspector] public float lastTimeJumped;

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
        BattleState = new MagicSkeletonBattleState(this, StateMachine, "Move", this);
        AttackState = new MagicSkeletonAttackState(this, StateMachine, "Attack", this);
        StunnedState = new MagicSkeletonStunnedState(this, StateMachine, "Stunned", this);
        JumpState = new MagicSkeletonJumpState(this, StateMachine, "Jump", this);

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
}
