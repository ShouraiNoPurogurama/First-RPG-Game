using UnityEngine;

namespace Enemies.Swordman
{
    public class EnemySwordmanGroundedState : EnemyState
    {
        protected EnemySwordman enemy;
        protected EnemySwordmanGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySwordman enemy) : base(enemyBase, stateMachine, animBoolName)
        {
            this.enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            base.Update();
            if (enemy.IsPlayerDetected()) 
            { 
                StateMachine.ChangeState(enemy.BattleState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }


    }

}
