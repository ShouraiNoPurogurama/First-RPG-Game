using Enemies.Swordman;
using UnityEngine;

namespace Enemies.Swordman
{
    public class EnemySwordmanBattleState : EnemyState
    {
        private Transform player;
        private EnemySwordman enemy;
        private int moveDir;
        public EnemySwordmanBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySwordman _enemy) : base(enemyBase, stateMachine, animBoolName )
        {
            this.enemy = _enemy;
        }

        public override void Enter()
        {
            base.Enter();

            player= GameObject.Find("Player").transform;

        }
        public override void Update()
        {
            base.Update();

            if (player.position.x > enemy.transform.position.x) 
            {
                moveDir = 1;
            }
            else if (player.position.x < enemy.transform.position.x)
            {
                moveDir = -1;
            }
            enemy.SetVelocity(enemy.moveSpeed * moveDir,Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }


    }
}
