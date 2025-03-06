using Enemies;

public class MagicSkeletonIdleState : MagicSkeletonGroundedState
{
    public MagicSkeletonIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Magic_Skeleton _magic_Skeleton_Enemy) : base(enemyBase, stateMachine, animBoolName, _magic_Skeleton_Enemy)
    {
    }
    public override void Enter()
    {
        base.Enter();

        StateTimer = MagicSkeleton.idleTime;
    }

    public override void Update()
    {
        if (MagicSkeleton.BattleState.PlayerInAttackRange() && MagicSkeleton.BattleState.CanAttack())
        {
            StateMachine.ChangeState(MagicSkeleton.AttackState);
        }

        base.Update();

        if (MagicSkeleton.BattleState.PlayerInAttackRange() && !MagicSkeleton.BattleState.CanAttack())
        {
            return;
        }

        if (StateTimer <= 0)
        {
            StateMachine.ChangeState(MagicSkeleton.MoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
