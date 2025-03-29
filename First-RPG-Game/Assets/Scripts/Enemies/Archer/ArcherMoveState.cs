using UnityEngine;

namespace Enemies.Archer
{
    public class ArcherMoveState : ArcherGroundedState
    {
        public ArcherMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher archer) :
            base(enemyBase, stateMachine, animBoolName, archer)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            
            Archer.SetVelocity(Archer.FacingDir * Archer.defaultMoveSpeed, Rb.linearVelocity.y);
            
            if (!Archer.IsBusy && (Archer.IsWallDetected() || !Archer.IsGroundDetected()))
            {
                Archer.Flip();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}