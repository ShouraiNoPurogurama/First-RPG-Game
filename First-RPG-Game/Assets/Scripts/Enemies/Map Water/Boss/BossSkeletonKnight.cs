using Enemies;
using Enemies.Map_Water.Boss;
using UnityEngine;

public class BossSkeletonKnight : Enemy
{
    #region
    public BossSkeletonKnightIdleState IdleState { get; private set; }
    public BossSkeletonKnightMoveState MoveState { get; private set; }
    public BossSkeletonKnightBattleState BattleState { get; private set; }
    public BossSkeletonKnightAttackState AttackState { get; private set; }
    public BossSkeletonKnightDeadState DeadState { get; private set; }
    public BossSkeletonKnightDisappearState DisappearState { get; private set; }
    public BossSkeletonKnightAppearState AppearState { get; private set; }
    #endregion

    [Header("Teleport details")]
    public float chanceToTeleport;
    public float defaultChanceToTeleport = 25;
    protected override void Awake()
    {
        base.Awake();

        IdleState = new BossSkeletonKnightIdleState(this, StateMachine, "Idle", this);
        MoveState = new BossSkeletonKnightMoveState(this, StateMachine, "Move", this);
        BattleState = new BossSkeletonKnightBattleState(this, StateMachine, "Idle", this);
        AttackState = new BossSkeletonKnightAttackState(this, StateMachine, "Attack", this);
        DeadState = new BossSkeletonKnightDeadState(this, StateMachine, "Idle", this);
        DisappearState = new BossSkeletonKnightDisappearState(this, StateMachine, "Disappear", this);
        AppearState = new BossSkeletonKnightAppearState(this, StateMachine, "Appear", this);
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

    public bool CanTeleport()
    {
        if (Random.Range(0, 100) <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }


        return false;
    }
}
