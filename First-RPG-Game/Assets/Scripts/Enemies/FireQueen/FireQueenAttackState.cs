using Audio;
using System;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Enemies.FireQueen
{
    public class FireQueenAttackState : EnemyState
    {
        private EnemyFireQueen fireQueen;
        public FireQueenAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireQueen _fireQueen) : base(enemyBase, stateMachine, animBoolName)
        {
            fireQueen = _fireQueen;
        }

        public override void Enter()
        {
            SoundManager.PlaySFX("FireQueen", 0, true);
            base.Enter();
        }
        public override void Update()
        {
            base.Update();

            fireQueen.SetZeroVelocity();

            if (TriggerCalled)
            {
                TriggerCalled = false;
                fireQueen.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(fireQueen.BattleState);
            }

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
