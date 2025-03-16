using System;
using UnityEngine;
using System.Collections;

namespace Enemies.Boss
{
    public class BossAttackState : EnemyState
    {
        private EnemyBoss boss;
        public BossAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyBoss _boss) : base(enemyBase, stateMachine, animBoolName)
        {
            boss = _boss;
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            base.Update();
            boss.SetZeroVelocity();

            if (TriggerCalled)
            {
                TriggerCalled = false;
                //boss.lastTimeAttacked = Time.time;
                boss.attackManager.HandleAttackLogic(); // Gọi Attack Manager
                StateMachine.ChangeState(boss.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
