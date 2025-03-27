using UnityEngine;

namespace Enemies.Boss
{
    public class BossGroundedState : EnemyState
    {
        protected readonly EnemyBoss boss;

        private Transform _player;

        protected BossGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyBoss _boss) : base(enemyBase, stateMachine, animBoolName)
        {
            boss = _boss;
        }

        public override void Enter()
        {
            base.Enter();

            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();

            if (boss.IsPlayerDetected() || Vector2.Distance(boss.transform.position, _player.position) < 10)
            {
                StateMachine.ChangeState(boss.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}
