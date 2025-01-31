using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected EnemySkeleton Skeleton;   
    public SkeletonGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton skeleton) : base(enemyBase, stateMachine, animBoolName)
    {
        Skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Skeleton.IsPlayerDetected())
        {
            StateMachine.ChangeState(Skeleton.BattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
