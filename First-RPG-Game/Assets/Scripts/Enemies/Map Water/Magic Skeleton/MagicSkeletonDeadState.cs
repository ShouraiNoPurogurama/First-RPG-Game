using UnityEngine;

namespace Enemies.Map_Water.Magic_Skeleton
{
    public class MagicSkeletonDeadState : EnemyState
    {
        private Enemy_Magic_Skeleton magicSkeleton;
        public MagicSkeletonDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Magic_Skeleton _enemy) : base(enemyBase, stateMachine, animBoolName)
        {
            magicSkeleton = _enemy;
        }
        public override void Enter()
        {
            base.Enter();

            magicSkeleton.Animator.SetBool(magicSkeleton.LastAnimBoolName, true);
            magicSkeleton.Animator.speed = 0;
            magicSkeleton.CapsuleCollider.enabled = false;

            StateTimer = .15f;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer > 0)
            {
                Rb.linearVelocity = new Vector2(0, 10);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
