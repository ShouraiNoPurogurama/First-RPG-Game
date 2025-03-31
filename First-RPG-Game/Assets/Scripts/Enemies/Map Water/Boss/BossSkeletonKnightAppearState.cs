using Enemies;

public class BossSkeletonKnightAppearState : EnemyState
{
    private BossSkeletonKnight enemy;

    public BossSkeletonKnightAppearState(
        Enemy enemyBase,
        EnemyStateMachine stateMachine,
        string animBoolName,
        BossSkeletonKnight enemy
    ) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (TriggerCalled)
        {
            StateMachine.ChangeState(enemy.BattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
