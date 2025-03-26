using UnityEngine;

namespace Enemies.FireMage
{
    public class FireMageMoveState : FireMageGroundedState
    {
        public FireMageMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMage _fireMage) : base(enemyBase, stateMachine, animBoolName, _fireMage)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();


            if (!fireMage.IsBusy && (fireMage.IsWallDetected() || !fireMage.IsGroundDetected()))
            {
                fireMage.Flip();
            }
            fireMage.SetVelocity(fireMage.FacingDir * fireMage.moveSpeed, fireMage.Rb.linearVelocity.y);

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
