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

            if (!fireWorm.IsBusy && (fireWorm.IsWallDetected() || !fireWorm.IsGroundDetected()))
            {
                fireWorm.Flip();
            }
            fireWorm.SetVelocity(fireWorm.FacingDir * fireWorm.moveSpeed, fireWorm.Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
