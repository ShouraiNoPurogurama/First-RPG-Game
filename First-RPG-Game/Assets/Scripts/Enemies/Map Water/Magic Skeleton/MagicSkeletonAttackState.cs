using UnityEngine;

namespace Enemies.Map_Water.Magic_Skeleton
{
    public class MagicSkeletonAttackState : EnemyState
    {
        private Enemy_Magic_Skeleton magicSkeleton;

        public MagicSkeletonAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Magic_Skeleton _enemy) : base(enemyBase, stateMachine, animBoolName)
        {
            magicSkeleton = _enemy;
        }
        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            magicSkeleton.SetZeroVelocity();

            if (TriggerCalled)
            {
                TriggerCalled = false;
                magicSkeleton.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(magicSkeleton.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
