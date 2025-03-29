using UnityEngine;

namespace Enemies.DarkBoss
{
    public class DarkBossTeleportState : EnemyState
    {
        private DarkBoss _darkBoss;
        public DarkBossTeleportState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, DarkBoss darkBoss) : base(enemyBase, stateMachine, animBoolName)
        {
            _darkBoss = darkBoss;
        }
        public override void Enter()
        {
            base.Enter();
            StateTimer = 1f;
            
        }

        public override void Update()
        {
            base.Update();
            if(TriggerCalled)
            {
                _darkBoss.lastTimeTeleport = Time.time;
                StateMachine.ChangeState(_darkBoss.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }



}
