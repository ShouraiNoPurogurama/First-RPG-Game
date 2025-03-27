using UnityEngine;

namespace Enemies.Boss
{
    public class BossMoveState : BossGroundedState
    {
        public BossMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyBoss _boss) : base(enemyBase, stateMachine, animBoolName, _boss)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();


            if (!boss.IsBusy && (boss.IsWallDetected() || !boss.IsGroundDetected()))
            {
                boss.Flip();
            }
            boss.SetVelocity(boss.FacingDir * boss.moveSpeed, boss.Rb.linearVelocity.y);

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
