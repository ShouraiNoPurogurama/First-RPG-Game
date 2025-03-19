using UnityEngine;

namespace Enemies.FireQueen
{
    public class FireQueenIdleState : FireQueenGroundedState
    {
        public FireQueenIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireQueen _fireQueen) : base(enemyBase, stateMachine, animBoolName, _fireQueen)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = fireQueen.idleTime;
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
                StateMachine.ChangeState(fireQueen.MoveState);
            }
         
            /*
            if (fireQueen.BattleState.PlayerInAttackRange() && fireQueen.BattleState.CanAttack())
            {
                StateMachine.ChangeState(fireQueen.TongueAttackState);
            }

            base.Update();

            if (fireQueen.BattleState.PlayerInAttackRange() && !fireQueen.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                Debug.Log("Change state");
                StateMachine.ChangeState(fireQueen.MoveState);
            }
        }*/

        public override void Update()
        {
            //Debug.Log("player: " + fireQueen.BattleState.PlayerInAttackRange());
            //Debug.Log("atk" + fireQueen.BattleState.CanAttack());
            if (fireQueen.BattleState.PlayerInAttackRange() && fireQueen.BattleState.CanAttack())
            {
                //.Log("attacking dsdsđsdsd");
                StateMachine.ChangeState(fireQueen.AttackState);
            }

            base.Update();

            if (fireQueen.BattleState.PlayerInAttackRange() && !fireQueen.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                //Debug.Log("Change state");
                StateMachine.ChangeState(fireQueen.MoveState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    

    }
}
