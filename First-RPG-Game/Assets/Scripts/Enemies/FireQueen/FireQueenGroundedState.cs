using UnityEngine;

namespace Enemies.FireQueen
{
    public class FireQueenGroundedState : EnemyState
    {
        protected readonly EnemyFireQueen fireQueen;

        private Transform _player;

        protected FireQueenGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireQueen _fireQueen) : base(enemyBase, stateMachine, animBoolName)
        {
            fireQueen = _fireQueen;
        }

        public override void Enter()
        {
            base.Enter();

            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();

            if (fireQueen.IsPlayerDetected() || Vector2.Distance(fireQueen.transform.position, _player.position) < 2)
            {
                StateMachine.ChangeState(fireQueen.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}
