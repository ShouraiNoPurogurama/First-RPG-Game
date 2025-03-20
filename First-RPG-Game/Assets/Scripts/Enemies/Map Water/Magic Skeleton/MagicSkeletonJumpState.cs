using UnityEngine;

namespace Enemies.Map_Water.Magic_Skeleton
{
    public class MagicSkeletonJumpState : EnemyState
    {
        private Enemy_Magic_Skeleton magicSkeleton;

        public MagicSkeletonJumpState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Magic_Skeleton enemy) : base(enemyBase, stateMachine, animBoolName)
        {
            magicSkeleton = enemy;
        }
        public override void Enter()
        {
            base.Enter();

            Rb.linearVelocity = new Vector2(magicSkeleton.jumpVelocity.x * -magicSkeleton.FacingDir, magicSkeleton.jumpVelocity.y);
        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void Update()
        {
            base.Update();

            magicSkeleton.Animator.SetFloat("yVelocity", Rb.linearVelocity.y);

            if (Rb.linearVelocity.y < 0 && magicSkeleton.IsGroundDetected())
            {
                StateMachine.ChangeState(magicSkeleton.BattleState);
            }
        }
    }
}
