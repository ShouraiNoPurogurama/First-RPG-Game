using UnityEngine;

namespace Enemies.FireWorm
{
    public class FireWormGroundedState : EnemyState
    {
        protected readonly EnemyFireWorm fireWorm;

        private Transform _player;

        protected FireWormGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireWorm _fireWorm) : base(enemyBase, stateMachine, animBoolName)
        {
            fireWorm = _fireWorm;
        }

        public override void Enter()
        {
            base.Enter();

            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();

            if (fireWorm.IsPlayerDetected() || Vector2.Distance(fireWorm.transform.position, _player.position) < 2)
            {
                StateMachine.ChangeState(fireWorm.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}
