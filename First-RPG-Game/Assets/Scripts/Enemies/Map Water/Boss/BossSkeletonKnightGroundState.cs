using UnityEngine;

namespace Enemies.Map_Water.Boss
{
    public class BossSkeletonKnightGroundState : EnemyState
    {
        private BossSkeletonKnight enemy;
        private Transform _player;
        public BossSkeletonKnightGroundState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, BossSkeletonKnight enemy) : base(enemyBase, stateMachine, animBoolName)
        {
            this.enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();

            if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, _player.position) < 2)
            {
                StateMachine.ChangeState(enemy.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
