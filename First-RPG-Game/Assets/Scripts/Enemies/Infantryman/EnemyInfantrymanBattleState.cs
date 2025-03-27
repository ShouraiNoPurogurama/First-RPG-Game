using UnityEngine;

namespace Enemies.Infantryman
{
    public class EnemyInfantrymanBattleState : EnemyState
    {
        private Transform player;
        private EnemyInfantryman enemy;
        private int moveDir;
        public EnemyInfantrymanBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyInfantryman _enemy) : base(enemyBase, stateMachine, animBoolName )
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
