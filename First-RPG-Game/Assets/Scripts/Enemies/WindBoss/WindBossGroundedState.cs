using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossGroundedState : EnemyState
    {
        protected readonly WindBoss WindBoss;

        protected Transform Player;

        protected WindBossGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) :
            base(enemyBase, stateMachine, animBoolName)
        {
            WindBoss = windBoss;
        }

        public override void Enter()
        {
            base.Enter();

            Player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();
            
            if (StateTimer <= 0 && (WindBoss.IsPlayerDetected() || Vector2.Distance(WindBoss.transform.position, Player.position) < WindBoss.attackDistance + 20))
            {
                Debug.Log("WindBossGroundedState: Player detected, changing state to WindBossBattleState");
                StateMachine.ChangeState(WindBoss.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}