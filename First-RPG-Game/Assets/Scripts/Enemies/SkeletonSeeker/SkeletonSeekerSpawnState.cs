namespace Enemies.SkeletonSeeker
{
    public class SkeletonSeekerSpawnState : EnemyState
    {
        private SkeletonSeeker _skeletonSeeker;
        public SkeletonSeekerSpawnState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, SkeletonSeeker skeletonSeeker) : base(enemyBase, stateMachine, animBoolName)
        {
            _skeletonSeeker = skeletonSeeker;
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = 1f;
        }

        public override void Update()
        {
            base.Update();
            if(StateTimer < 0)
            {
                StateMachine.ChangeState(_skeletonSeeker.IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
