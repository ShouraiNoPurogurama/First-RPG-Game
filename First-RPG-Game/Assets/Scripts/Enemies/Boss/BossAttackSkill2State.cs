using System;
using UnityEngine;
using System.Collections;
using Audio;

namespace Enemies.Boss
{
    public class BossAttackSkill2State : EnemyState
    {
        private EnemyBoss boss;
        public BossAttackSkill2State(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyBoss _boss) : base(enemyBase, stateMachine, animBoolName)
        {
            boss = _boss;
        }

        public override void Enter()
        {
            base.Enter();
            SoundManager.PlaySFX("FireBoss", 4, true);
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
