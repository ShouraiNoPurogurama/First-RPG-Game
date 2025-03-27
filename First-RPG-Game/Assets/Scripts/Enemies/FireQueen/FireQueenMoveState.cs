using UnityEngine;

namespace Enemies.FireQueen
{
    public class FireQueenMoveState : FireQueenGroundedState
    {
        public FireQueenMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireQueen _fireQueen) : base(enemyBase, stateMachine, animBoolName, _fireQueen)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();


            if (!fireQueen.IsBusy && (fireQueen.IsWallDetected() || !fireQueen.IsGroundDetected()))
            {
                fireQueen.Flip();
            }
            fireQueen.SetVelocity(fireQueen.FacingDir * fireQueen.moveSpeed, fireQueen.Rb.linearVelocity.y);

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
