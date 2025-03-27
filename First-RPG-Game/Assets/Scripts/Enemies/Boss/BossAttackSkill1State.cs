using System;
using UnityEngine;
using System.Collections;

namespace Enemies.Boss
{
    public class BossAttackSkill1State : EnemyState
    {
        private EnemyBoss boss;
        public BossAttackSkill1State(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyBoss _boss) : base(enemyBase, stateMachine, animBoolName)
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

            boss.attackManager.HandleAttackLogic();

            boss.SetZeroVelocity();

            if (TriggerCalled)
            {
                TriggerCalled = false;
                boss.lastTimeAttacked = Time.time;
                boss.attackCooldown = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
                //boss.attackManager.HandleAttackLogic(); // Gọi Attack Manager
                StateMachine.ChangeState(boss.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
