using UnityEngine;

namespace Enemies.FireSpider
{
    public class FireSpiderIdleState : FireSpiderGroundedState
    {
        public FireSpiderIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSpider _fireSpider) : base(enemyBase, stateMachine, animBoolName, _fireSpider)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = fireSpider.idleTime;
        }
        /*
        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            Debug.Log("loi");
            base.Update();
            
            if (StateTimer <= 0)
            {
                Debug.Log("loi move");
                StateMachine.ChangeState(fireSpider.MoveState);
            }
         
            /*
            if (fireSpider.BattleState.PlayerInAttackRange() && fireSpider.BattleState.CanAttack())
            {
                StateMachine.ChangeState(fireSpider.AttackState);
            }

            base.Update();

            if (fireSpider.BattleState.PlayerInAttackRange() && !fireSpider.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                Debug.Log("Change state");
                StateMachine.ChangeState(fireSpider.MoveState);
            }
        }*/

        public override void Update()
        {
            //Debug.Log("player: " + fireSpider.BattleState.PlayerInAttackRange());
            //Debug.Log("atk" + fireSpider.BattleState.CanAttack());
            if (fireSpider.BattleState.PlayerInAttackRange() && fireSpider.BattleState.CanAttack())
            {
                //.Log("attacking dsdsđsdsd");
                StateMachine.ChangeState(fireSpider.AttackState);
            }

            base.Update();

            if (fireSpider.BattleState.PlayerInAttackRange() && !fireSpider.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                //Debug.Log("Change state");
                StateMachine.ChangeState(fireSpider.MoveState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    

    }
}
