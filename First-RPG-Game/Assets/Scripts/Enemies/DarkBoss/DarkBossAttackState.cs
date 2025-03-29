using UnityEngine;

namespace Enemies.DarkBoss
{
    public class DarkBossAttackState : EnemyState
    {
        private DarkBoss _darkBoss;
        public DarkBossAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, DarkBoss darkBoss) : base(enemyBase, stateMachine, animBoolName)
        {
            _darkBoss = darkBoss;
        }
        public override void Update()
        {
            base.Update();

            _darkBoss.SetZeroVelocity();

            if (TriggerCalled)
            {
                TriggerCalled = false;
                _darkBoss.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_darkBoss.BattleState);
                //StateMachine.ChangeState(_darkBoss.TeleportState);
            }
        }
        public override void Enter()
        {
            base.Enter();
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
}
