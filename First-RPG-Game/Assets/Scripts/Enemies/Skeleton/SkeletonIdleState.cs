using UnityEngine;

namespace Enemies.Skeleton
{
    public class SkeletonIdleState : SkeletonGroundedState
    {
        public SkeletonIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton skeleton) :
            base(enemyBase, stateMachine, animBoolName, skeleton)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StateTimer = Skeleton.idleTime;
        }

        public override void Update()
        {
            if (Skeleton.BattleState.PlayerInAttackRange() && Skeleton.BattleState.CanAttack())
            {
                StateMachine.ChangeState(Skeleton.AttackState);
            }
            
            base.Update();

            if (Skeleton.BattleState.PlayerInAttackRange() && !Skeleton.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(Skeleton.MoveState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}