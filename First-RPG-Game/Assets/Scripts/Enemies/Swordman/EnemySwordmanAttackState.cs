using Enemies.Swordman;
using UnityEngine;

namespace Enemies.Swordman
{
    public class EnemySwordmanAttackState : EnemyState
    {
        private EnemySwordman enemy;
        public EnemySwordmanAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySwordman _enemy) : base(enemyBase, stateMachine, animBoolName )
        {
            this.enemy = _enemy;
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            base.Update();
            enemy.SetZeroVelocity();

            if(TriggerCalled)
            {
                TriggerCalled = false;
                enemy.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(enemy.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            //enemy.lastTimeAttacked = Time.time;
        }


    }
}
