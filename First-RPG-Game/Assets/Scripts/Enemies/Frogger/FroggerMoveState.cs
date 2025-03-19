using Enemies.Frogger;
using UnityEngine;

namespace Enemies.Frogger
{
    public class FroggerMoveState : FroggerGroundedState
    {
        public FroggerMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFrogger Frogger) :
            base(enemyBase, stateMachine, animBoolName, Frogger)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            
            Frogger.SetVelocity(Frogger.FacingDir * Frogger.defaultMoveSpeed, Rb.linearVelocity.y);
            
            if (!Frogger.IsBusy && (Frogger.IsWallDetected() || !Frogger.IsGroundDetected()))
            {
                Frogger.Flip();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}