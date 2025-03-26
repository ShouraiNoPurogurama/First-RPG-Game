using UnityEngine;

namespace Enemies.FireSnake
{
    public class FireSnakeMoveState : FireSnakeGroundedState
    {
        public FireSnakeMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSnake _fireSnake) : base(enemyBase, stateMachine, animBoolName, _fireSnake)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();


            if (!fireSnake.IsBusy && (fireSnake.IsWallDetected() || !fireSnake.IsGroundDetected()))
            {
                fireSnake.Flip();
            }
            fireSnake.SetVelocity(fireSnake.FacingDir * fireSnake.moveSpeed, fireSnake.Rb.linearVelocity.y);

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
