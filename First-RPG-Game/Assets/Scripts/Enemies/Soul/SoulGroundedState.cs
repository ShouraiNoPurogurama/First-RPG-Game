using UnityEngine;

namespace Enemies.Soul
{
    public class SoulGroundedState : EnemyState
    {
        protected readonly EnemySoul Soul;

        private Transform _player;

        protected SoulGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySoul soul) :
            base(enemyBase, stateMachine, animBoolName)
        {
            Soul = soul;
        }

        public override void Enter()
        {
            base.Enter();

            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();
            
            if (StateTimer <= 0 && (Soul.IsPlayerDetected() || Vector2.Distance(Soul.transform.position, _player.position) < Soul.attackDistance + 5))
            {
                StateMachine.ChangeState(Soul.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}