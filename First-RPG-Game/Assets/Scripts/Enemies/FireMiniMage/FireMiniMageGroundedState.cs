using UnityEngine;

namespace Enemies.FireMiniMage
{
    public class FireMiniMageGroundedState : EnemyState
    {
        protected readonly EnemyFireMiniMage FireMiniMage;

        private Transform _player;

        protected FireMiniMageGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMiniMage _FireMiniMage) : base(enemyBase, stateMachine, animBoolName)
        {
            FireMiniMage = _FireMiniMage;
        }

        public override void Enter()
        {
            base.Enter();

            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();

            if (FireMiniMage.IsPlayerDetected() || Vector2.Distance(FireMiniMage.transform.position, _player.position) < 2)
            {
                StateMachine.ChangeState(FireMiniMage.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}
