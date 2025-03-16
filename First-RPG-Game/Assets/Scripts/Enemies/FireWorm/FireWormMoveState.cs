using UnityEngine;

namespace Enemies.FireWorm
{
    public class FireWormMoveState : FireWormGroundedState
    {
        public FireWormMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireWorm _fireWorm) : base(enemyBase, stateMachine, animBoolName, _fireWorm)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            fireWorm.SetVelocity(fireWorm.FacingDir * fireWorm.moveSpeed, fireWorm.Rb.linearVelocity.y);

            if (!fireWorm.IsBusy && (fireWorm.IsWallDetected() || !fireWorm.IsGroundDetected()))
            {
                fireWorm.Flip();
            }

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
