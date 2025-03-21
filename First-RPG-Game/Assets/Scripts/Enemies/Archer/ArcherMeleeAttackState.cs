using UnityEngine;

namespace Enemies.Archer
{
    public class ArcherMeleeAttackState : EnemyState
    {
        private EnemyArcher _archer;

        public ArcherMeleeAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher enemyArcher) : base(enemyBase, stateMachine, animBoolName)
        {
            _archer = enemyArcher;
        }
        
        public override void Update()
        {
            base.Update();
            
            if (TriggerCalled)
            {
                TriggerCalled = false;
                _archer.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_archer.BattleState);
            }
        }
    }
}