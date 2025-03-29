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

            WindBoss.SetVelocity(WindBoss.FacingDir * WindBoss.defaultMoveSpeed, Rb.linearVelocity.y);

            if (!WindBoss.IsBusy && (WindBoss.IsWallDetected() || !WindBoss.IsGroundDetected()))
            {
                WindBoss.Flip();
            }

            if (Mathf.Abs(Player.transform.position.x - WindBoss.transform.position.x) > 5f)
            {
                FaceToPlayer();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void FaceToPlayer()
        {
            if (Player.transform.position.x > WindBoss.transform.position.x && WindBoss.FacingDir == -1)
                WindBoss.Flip();
            else if (Player.transform.position.x < WindBoss.transform.position.x && WindBoss.FacingDir == 1)
                WindBoss.Flip();
        }
    }
}