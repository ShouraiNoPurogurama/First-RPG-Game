using UnityEngine;

namespace Enemies.SkeletonSeeker
{
    public class SkeletonSeekerAttackState : EnemyState
    {
        private SkeletonSeeker _skeletonSeeker;
        public SkeletonSeekerAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, SkeletonSeeker skeletonSeeker) : base(enemyBase, stateMachine, animBoolName)
        {
            _skeletonSeeker = skeletonSeeker;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            _skeletonSeeker.SetZeroVelocity();

            if (TriggerCalled)
            {
                TriggerCalled = false;
                _skeletonSeeker.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_skeletonSeeker.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
