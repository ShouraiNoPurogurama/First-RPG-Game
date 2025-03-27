using Enemies.Infantryman;
using UnityEngine;

namespace Enemies.Infantryman
{
    public class EnemyInfantrymanGroundedState : EnemyState
    {
        protected EnemyInfantryman enemy;
        protected Transform player;
        protected EnemyInfantrymanGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyInfantryman enemy) : base(enemyBase, stateMachine, animBoolName)
        {
            this.enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();
            player = GameObject.Find("Player").transform;
        }
        public override void Update()
        {
            base.Update();
            //if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
            //{
            //    StateMachine.ChangeState(enemy.BattleState);
            //}
        }
        public override void Exit()
        {
            base.Exit();
        }


    }

}
