namespace Enemies.Map_Water.Magic_Skeleton
{
    public class MagicSkeletonMoveState : MagicSkeletonGroundedState
    {
        public MagicSkeletonMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Magic_Skeleton _magic_Skeleton_Enemy) : base(enemyBase, stateMachine, animBoolName, _magic_Skeleton_Enemy)
        {
        }
        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            MagicSkeleton.SetVelocity(MagicSkeleton.FacingDir * MagicSkeleton.moveSpeed, Rb.linearVelocity.y);

            if (!MagicSkeleton.IsBusy && (MagicSkeleton.IsWallDetected() || !MagicSkeleton.IsGroundDetected()))
            {
                MagicSkeleton.Flip();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}