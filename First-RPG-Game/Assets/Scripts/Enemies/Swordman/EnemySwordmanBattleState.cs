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
            if (enemy.IsPlayerDetected())
            { 
                StateTimer = enemy.battleTime;
                if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
                {
                    if (CanAttack())
                        StateMachine.ChangeState(enemy.AttackState);


                }
            }
            else
            { 
                if(StateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position)>8)
                    StateMachine.ChangeState(enemy.IdleState);
            }


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
        private bool CanAttack()
        {
            if (Time.time > enemy.lastTimeAttacked + enemy.attackCooldown)
            {
                enemy.lastTimeAttacked = Time.time;
                return true;

            }
            return false;
        }

    }
}
