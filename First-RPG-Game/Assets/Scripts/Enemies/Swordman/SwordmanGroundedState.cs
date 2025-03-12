using UnityEngine;

namespace Enemies.Swordman
{
    public class SwordmanGroundedState : EnemyState
    {
        protected readonly EnemySwordman Swordman;

        private Transform _player;

        protected SwordmanGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySwordman swordman) : base(enemyBase, stateMachine, animBoolName)
        {
            Swordman = swordman;
        }

        public override void Enter()
        {
            base.Enter();

            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();
            
            //if (Swordman.IsPlayerDetected() || Vector2.Distance(Swordman.transform.position, _player.position) < 2)
            //{
            //    StateMachine.ChangeState(Swordman.BattleState);
            //}
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
