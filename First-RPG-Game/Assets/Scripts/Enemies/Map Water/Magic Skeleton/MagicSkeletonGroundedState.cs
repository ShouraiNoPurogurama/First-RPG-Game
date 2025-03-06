using Enemies;
using UnityEngine;

public class MagicSkeletonGroundedState : EnemyState
{
    protected Transform _player;
    protected Enemy_Magic_Skeleton MagicSkeleton;

    public MagicSkeletonGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Magic_Skeleton _magic_Skeleton_Enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        MagicSkeleton = _magic_Skeleton_Enemy;
    }

    public override void Enter()
    {
        base.Enter();

        _player = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        base.Update();

        if (MagicSkeleton.IsPlayerDetected() || Vector2.Distance(MagicSkeleton.transform.position, _player.position) < 2)
        {
            StateMachine.ChangeState(MagicSkeleton.BattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
