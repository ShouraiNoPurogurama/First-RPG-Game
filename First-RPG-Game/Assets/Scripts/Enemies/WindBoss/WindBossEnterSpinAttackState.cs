using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossEnterSpinAttackState : EnemyState
    {
        private WindBoss _windBoss;

        public WindBossEnterSpinAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) : base(enemyBase, stateMachine, animBoolName)
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
                StateMachine.ChangeState(_windBoss.SpinAttackState);
            }
        }
    }
}