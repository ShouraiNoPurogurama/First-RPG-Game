namespace Enemies.SkeletonSeeker
{
    public class SkeletonSeekerMoveState : SkeletonSeekerGroundedState
    {
        public SkeletonSeekerMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, SkeletonSeeker skeletonSeeker) : base(enemyBase, stateMachine, animBoolName, skeletonSeeker)
        {
        }
        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            SkeletonSeeker.SetVelocity(SkeletonSeeker.FacingDir * SkeletonSeeker.moveSpeed, Rb.linearVelocity.y);

            if (!SkeletonSeeker.IsBusy && (SkeletonSeeker.IsWallDetected() || !SkeletonSeeker.IsGroundDetected()))
            {
                SkeletonSeeker.Flip();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
