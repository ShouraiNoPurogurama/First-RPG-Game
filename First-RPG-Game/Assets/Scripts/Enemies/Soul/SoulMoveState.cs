using UnityEngine;

namespace Enemies.Soul
{
    public class SoulMoveState : SoulGroundedState
    {
        public SoulMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySoul soul) :
            base(enemyBase, stateMachine, animBoolName, soul)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            
            Soul.SetVelocity(Soul.FacingDir * Soul.defaultMoveSpeed, Rb.linearVelocity.y);
            
            if (!Soul.IsBusy && (Soul.IsWallDetected() || !Soul.IsGroundDetected()))
            {
                Soul.Flip();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}