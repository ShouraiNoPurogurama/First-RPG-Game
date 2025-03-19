using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossMeleeAttackState : EnemyState
    {
        private WindBoss _windBoss;

        public WindBossMeleeAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) : base(enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }
        
        public override void Update()
        {
            base.Update();
            
            if (TriggerCalled)
            {
                TriggerCalled = false;
                _windBoss.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_windBoss.BattleState);
            }
        }
    }
}