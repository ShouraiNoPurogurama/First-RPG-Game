public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton skeleton) : base(enemyBase, stateMachine, animBoolName, skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        Skeleton.SetVelocity(Skeleton.FacingDir * Skeleton.moveSpeed, Skeleton.Rb.linearVelocity.y);

        if (Skeleton.IsWallDetected() || !Skeleton.IsGroundDetected())
        {
            Skeleton.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
