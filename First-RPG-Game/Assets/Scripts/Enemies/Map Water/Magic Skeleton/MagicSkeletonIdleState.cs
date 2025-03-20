using System.Diagnostics;

namespace Enemies.Map_Water.Magic_Skeleton
{
    public class MagicSkeletonIdleState : MagicSkeletonGroundedState
    {
        public MagicSkeletonIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Magic_Skeleton _magic_Skeleton_Enemy) : base(enemyBase, stateMachine, animBoolName, _magic_Skeleton_Enemy)
        {
        }
        public override void Enter()
        {
            base.Enter();

            StateTimer = MagicSkeleton.idleTime;
        }

        public override void Update()
        {
            if (MagicSkeleton.BattleState.PlayerInAttackRange() && MagicSkeleton.BattleState.CanAttack())
            {
                Debug.WriteLine("MagicSkeletonIdleState: Magic Skeleton Attack State");
                StateMachine.ChangeState(MagicSkeleton.AttackState);
                return;
            }

            base.Update();

            if (MagicSkeleton.BattleState.PlayerInAttackRange() && !MagicSkeleton.BattleState.CanAttack())
            {
                Debug.WriteLine("MagicSkeletonIdleState: Magic Skeleton return");
                return;
            }

            if (StateTimer < 0)
            {
                Debug.WriteLine("MagicSkeletonIdleState: Magic Skeleton Move State");
                StateMachine.ChangeState(MagicSkeleton.MoveState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
