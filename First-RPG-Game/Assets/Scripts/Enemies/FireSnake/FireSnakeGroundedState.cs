using UnityEngine;

namespace Enemies.FireSnake
{
    public class FireSnakeGroundedState : EnemyState
    {
        protected readonly EnemyFireSnake fireSnake;

        private Transform _player;

        protected FireSnakeGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSnake _fireSnake) : base(enemyBase, stateMachine, animBoolName)
        {
            fireSnake = _fireSnake;
        }

        public override void Enter()
        {
            base.Enter();

            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();

            if (fireSnake.IsPlayerDetected() || Vector2.Distance(fireSnake.transform.position, _player.position) < 2)
            {
                StateMachine.ChangeState(fireSnake.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}
