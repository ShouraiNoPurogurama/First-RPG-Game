using UnityEngine;

namespace Enemies.DarkBoss
{
    public class DarkBossGroundedState : EnemyState
    {
        public DarkBoss DarkBoss;
        private Transform _player;

        public DarkBossGroundedState(Enemy enemy, EnemyStateMachine stateMachine, string animationBoolName, DarkBoss darkBoss) : base(enemy, stateMachine, animationBoolName)
        {
            this.DarkBoss = darkBoss;
        }
        public override void Enter()
        {
            base.Enter();
            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();

            if(DarkBoss.CanTeleport())
            {
                StateMachine.ChangeState(DarkBoss.TeleportState);
            }

            if (DarkBoss.IsPlayerDetected() || Vector2.Distance(DarkBoss.transform.position, _player.position) < 2)
            {
                StateMachine.ChangeState(DarkBoss.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        
    }
}
