using UnityEngine;

namespace Enemies.FireMiniMage
{
    public class FireMiniMageMoveState : FireMiniMageGroundedState
    {
        public FireMiniMageMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMiniMage _FireMiniMage) : base(enemyBase, stateMachine, animBoolName, _FireMiniMage)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            FireMiniMage.SetVelocity(FireMiniMage.FacingDir * FireMiniMage.moveSpeed, FireMiniMage.Rb.linearVelocity.y);

            if (!FireMiniMage.IsBusy && (FireMiniMage.IsWallDetected() || !FireMiniMage.IsGroundDetected()))
            {
                FireMiniMage.Flip();
            }

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
