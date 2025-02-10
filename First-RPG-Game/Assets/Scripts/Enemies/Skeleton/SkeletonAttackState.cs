using UnityEngine;

namespace Enemies.Skeleton
{
    public class SkeletonAttackState : EnemyState
    {
        private EnemySkeleton _skeleton;
    
        public SkeletonAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton skeleton) : base(enemyBase, stateMachine, animBoolName)
        {
            _skeleton = skeleton;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            
            _skeleton.SetZeroVelocity();
            
            if (TriggerCalled)
            {
                TriggerCalled = false;
                _skeleton.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_skeleton.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
