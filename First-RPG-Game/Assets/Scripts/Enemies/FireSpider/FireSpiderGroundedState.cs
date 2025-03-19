using UnityEngine;

namespace Enemies.FireSpider
{
    public class FireSpiderGroundedState : EnemyState
    {
        protected readonly EnemyFireSpider fireSpider;

        private Transform _player;

        protected FireSpiderGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSpider _fireSpider) : base(enemyBase, stateMachine, animBoolName)
        {
            fireSpider = _fireSpider;
        }

        public override void Enter()
        {
            base.Enter();

            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();

            if (fireSpider.IsPlayerDetected() || Vector2.Distance(fireSpider.transform.position, _player.position) < 2)
            {
                StateMachine.ChangeState(fireSpider.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}
