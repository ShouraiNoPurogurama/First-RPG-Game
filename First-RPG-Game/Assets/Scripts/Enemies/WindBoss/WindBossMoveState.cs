using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossMoveState : WindBossGroundedState
    {
        public WindBossMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) :
            base(enemyBase, stateMachine, animBoolName, windBoss)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            Debug.Log(WindBoss.defaultMoveSpeed);
            
            WindBoss.SetVelocity(WindBoss.FacingDir * WindBoss.defaultMoveSpeed, Rb.linearVelocity.y);
            
            if (!WindBoss.IsBusy && (WindBoss.IsWallDetected() || !WindBoss.IsGroundDetected()))
            {
                WindBoss.Flip();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}