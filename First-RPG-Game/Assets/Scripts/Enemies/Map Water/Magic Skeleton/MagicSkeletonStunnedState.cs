using UnityEngine;

namespace Enemies.Map_Water.Magic_Skeleton
{
    public class MagicSkeletonStunnedState : EnemyState
    {
        private Enemy_Magic_Skeleton MagicSkeleton;
        public MagicSkeletonStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Magic_Skeleton magic_Skeleton) : base(enemyBase, stateMachine, animBoolName)
        {
            MagicSkeleton = magic_Skeleton;
        }
        public override void Enter()
        {
            base.Enter();

            MagicSkeleton.FX.InvokeRepeating("RedColorBlink", 0, .1f);

            StateTimer = MagicSkeleton.stunDuration;

            Rb.linearVelocity = new Vector2(-MagicSkeleton.FacingDir * MagicSkeleton.stunDirection.x, MagicSkeleton.stunDirection.y);
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
                StateMachine.ChangeState(MagicSkeleton.IdleState);
        }

        public override void Exit()
        {
            base.Exit();

            MagicSkeleton.FX.Invoke("CancelRedBlink", 0);
        }
    }
}
