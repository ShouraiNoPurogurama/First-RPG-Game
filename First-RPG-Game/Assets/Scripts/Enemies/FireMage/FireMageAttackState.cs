using Audio;
using System;
using UnityEngine;

namespace Enemies.FireMage
{
    public class FireMageAttackState : EnemyState
    {
        private EnemyFireMage fireMage;
        public FireMageAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMage _fireMage) : base(enemyBase, stateMachine, animBoolName)
        {
            fireMage = _fireMage;
        }

        public override void Enter()
        {
            SoundManager.PlaySFX("FireMage", 0, true);
            base.Enter();
        }
        public override void Update()
        {
            base.Update();

            fireMage.SetZeroVelocity();

            if (TriggerCalled)
            {
                TriggerCalled = false;
                fireMage.lastTimeAttacked = Time.time;
                fireMage.SpawnFireballs();
                StateMachine.ChangeState(fireMage.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
