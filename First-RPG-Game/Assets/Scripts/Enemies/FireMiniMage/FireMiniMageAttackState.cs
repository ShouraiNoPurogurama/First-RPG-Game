using System;
using UnityEngine;

namespace Enemies.FireMiniMage
{
    public class FireMiniMageAttackState : EnemyState
    {
        private EnemyFireMiniMage FireMiniMage;
        public FireMiniMageAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMiniMage _FireMiniMage) : base(enemyBase, stateMachine, animBoolName)
        {
            FireMiniMage = _FireMiniMage;
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {

            base.Update();

            if (FireMiniMage.IsPlayerDetected().distance != 0 && FireMiniMage.IsPlayerDetected().distance <= FireMiniMage.attackDistance) 
            {
                if (CanThrowAttack())
                {
                    StateMachine.ChangeState(FireMiniMage.ThrowAttackState);
                    return;
                }
            }

            FireMiniMage.SetZeroVelocity();
            
            if (TriggerCalled)
            {
                TriggerCalled = false;
                FireMiniMage.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(FireMiniMage.BattleState);
            }
        }
        private bool CanThrowAttack()
        {
            return Time.time >= FireMiniMage.lastTimeAttacked + FireMiniMage.attackCooldown && FireMiniMage.IsPlayerDetected().distance <= FireMiniMage.throwballDistance;
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
}
