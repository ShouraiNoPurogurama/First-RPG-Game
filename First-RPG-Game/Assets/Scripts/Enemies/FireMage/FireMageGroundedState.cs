using UnityEngine;

namespace Enemies.FireMage
{
    public class FireMageGroundedState : EnemyState
    {
        protected readonly EnemyFireMage fireMage;

        private Transform _player;

        protected FireMageGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMage _fireMage) : base(enemyBase, stateMachine, animBoolName)
        {
            fireMage = _fireMage;
        }

        public override void Enter()
        {
            base.Enter();

            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();

            if (fireMage.IsPlayerDetected() || Vector2.Distance(fireMage.transform.position, _player.position) < 2)
            {
                StateMachine.ChangeState(fireMage.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}
