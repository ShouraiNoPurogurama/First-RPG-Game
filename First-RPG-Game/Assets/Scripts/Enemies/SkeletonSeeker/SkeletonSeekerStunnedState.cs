namespace Enemies.SkeletonSeeker
{
    public class SkeletonSeekerStunnedState : EnemyState
    {
        private SkeletonSeeker _skeletonSeeker;
        public SkeletonSeekerStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, SkeletonSeeker skeletonSeeker) : base(enemyBase, stateMachine, animBoolName)
        {
            _skeletonSeeker = skeletonSeeker;
        }
    }
}
