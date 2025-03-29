namespace Enemies.SkeletonSeeker
{
    public class SkeletonSeekerIdleState : SkeletonSeekerGroundedState
    {
        public SkeletonSeekerIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, SkeletonSeeker skeletonSeeker) : base(enemyBase, stateMachine, animBoolName, skeletonSeeker)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StateTimer = SkeletonSeeker.idleTime;
        }

        public override void Update()
        {
            base.Update();

            //if (StateTimer <= 0)
            //{
            //    StateMachine.ChangeState(SkeletonSeeker.MoveState);
            //}
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
