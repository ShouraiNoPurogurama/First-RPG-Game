using UnityEngine;

namespace Enemies.FireSpider
{
    public class FireSpiderMoveState : FireSpiderGroundedState
    {
        public FireSpiderMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSpider _fireSpider) : base(enemyBase, stateMachine, animBoolName, _fireSpider)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            fireSpider.SetVelocity(fireSpider.FacingDir * fireSpider.moveSpeed, fireSpider.Rb.linearVelocity.y);

            if (!fireSpider.IsBusy && (fireSpider.IsWallDetected() || !fireSpider.IsGroundDetected()))
            {
                fireSpider.Flip();
            }

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
