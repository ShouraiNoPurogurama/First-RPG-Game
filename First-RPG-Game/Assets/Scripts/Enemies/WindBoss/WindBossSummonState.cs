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

        public override void Enter()
        {
            base.Enter();
            Debug.Log("WindBossSummonState started");
            StateTimer = 1.5f;
            _windBoss.SetZeroVelocity();
        }

        public override void Update()
        {
            base.Update();
            
            if (StateTimer <= 0)
            {
                _windBoss.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_windBoss.BattleState);
            }
        }

        public override void Exit()
        {
            _windBoss.lastTimeSummon = Time.time;
            base.Exit();
        }
    }
}