using UnityEngine;

namespace Enemies.Soul
{
    public class SoulAttackState : EnemyState
    {
        private EnemySoul _soul;

        public SoulAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySoul enemy) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _soul = enemy;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            
            // if (_soul.IsPlayerDetected().distance < _soul.explodeDistance)
            // {
            //     //TODO
            // }

            _soul.SetZeroVelocity();
            
            if (TriggerCalled)
            {
                TriggerCalled = false;
                _soul.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_soul.BattleState);
            }
        }

        private bool CanMeleeAttack()
        {
            return Time.time >= _soul.lastTimeAttacked + _soul.attackCooldown && _soul.IsPlayerDetected().distance < 2f;
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}