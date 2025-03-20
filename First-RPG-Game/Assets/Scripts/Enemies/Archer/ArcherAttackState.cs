using UnityEngine;

namespace Enemies.Archer
{
    public class ArcherAttackState : EnemyState
    {
        private EnemyArcher _archer;

        public ArcherAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher enemy) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _archer = enemy;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            
            if (_archer.IsPlayerDetected().distance != 0 && _archer.IsPlayerDetected().distance < _archer.safeDistance)
            {
                if (CanJump())
                {
                    StateMachine.ChangeState(_archer.JumpState);
                    return;
                }
                if (CanMeleeAttack())
                {
                    StateMachine.ChangeState(_archer.MeleeAttackState);
                    return;
                }
            }

            _archer.SetZeroVelocity();
            
            if (TriggerCalled)
            {
                TriggerCalled = false;
                _archer.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_archer.BattleState);
            }
        }

        private bool CanMeleeAttack()
        {
            return Time.time >= _archer.lastTimeAttacked + _archer.attackCooldown && _archer.IsPlayerDetected().distance < 2f;
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        private bool CanJump()
        {
            if (_archer.GroundBehindCheck() && Time.time >= _archer.lastTimeJumped + _archer.jumpCooldown)
            {
                
                return true;
            }

            return false;
        }
    }
}