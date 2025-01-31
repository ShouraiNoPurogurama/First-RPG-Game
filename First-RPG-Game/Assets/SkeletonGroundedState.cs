using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected EnemySkeleton Skeleton;

    protected Transform Player;
    public SkeletonGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton skeleton) : base(enemyBase, stateMachine, animBoolName)
    {
        Skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();

        Player = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        base.Update();

        if (Skeleton.IsPlayerDetected() || Vector2.Distance(Skeleton.transform.position, Player.position) < 2)
        {
            StateMachine.ChangeState(Skeleton.BattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
