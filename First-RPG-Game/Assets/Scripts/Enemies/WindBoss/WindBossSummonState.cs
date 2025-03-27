using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossSummonState : EnemyState
    {
        private WindBoss _windBoss;

        public WindBossSummonState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) :
            base(enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }

        void Start()
        {
            StateTimer = 2;
            _windBoss.SetZeroVelocity();
        }

        public override void Update()
        {
            StateTimer -= Time.deltaTime;
            
            if (TriggerCalled || StateTimer <= 0)
            {
                TriggerCalled = false;
                _windBoss.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_windBoss.BattleState);
            }
        }

        public override void Exit()
        {
            TriggerCalled = false;
            _windBoss.lastTimeSummon = Time.time;
            base.Exit();
        }
    }
}