using UnityEngine;

namespace Enemies.Giant
{
    public class GiantGroundedState : EnemyState
    {
        protected Giant Giant;
        private Transform _player;
        protected GiantGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Giant giant) : base(enemyBase, stateMachine, animBoolName)
        {
            Giant = giant;
        }
        public override void Enter()
        {
            base.Enter();
            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();

            if (Giant.IsPlayerDetected() || Vector2.Distance(Giant.transform.position, _player.position) < 2)
            {
                StateMachine.ChangeState(Giant.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}
