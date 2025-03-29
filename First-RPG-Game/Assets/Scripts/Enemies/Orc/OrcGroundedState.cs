using UnityEngine;

namespace Enemies.Orc
{
    public class OrcGroundedState : EnemyState
    {
        protected readonly Orc Orc;

        private Transform _player;

        protected OrcGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Orc orc) : base(enemyBase, stateMachine, animBoolName)
        {
            Orc = orc;
        }

        public override void Enter()
        {
            base.Enter();

            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();

            if (Orc.IsPlayerDetected() || Vector2.Distance(Orc.transform.position, _player.position) < 2)
            {
                StateMachine.ChangeState(Orc.battleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
