using UnityEngine;

namespace Enemies.FireSnake
{
    public class FireSnakeIdleState : FireSnakeGroundedState
    {
        public FireSnakeIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSnake _fireSnake) : base(enemyBase, stateMachine, animBoolName, _fireSnake)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = fireSnake.idleTime;
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
                StateMachine.ChangeState(fireSnake.MoveState);
            }
         
            /*
            if (fireSnake.BattleState.PlayerInAttackRange() && fireSnake.BattleState.CanAttack())
            {
                StateMachine.ChangeState(fireSnake.AttackState);
            }

            base.Update();

            if (fireSnake.BattleState.PlayerInAttackRange() && !fireSnake.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                Debug.Log("Change state");
                StateMachine.ChangeState(fireSnake.MoveState);
            }
        }*/

        public override void Update()
        {
            //Debug.Log("player: " + fireSnake.BattleState.PlayerInAttackRange());
            //Debug.Log("atk" + fireSnake.BattleState.CanAttack());
            if (fireSnake.BattleState.PlayerInAttackRange() && fireSnake.BattleState.CanAttack())
            {
                //.Log("attacking dsdsđsdsd");
                StateMachine.ChangeState(fireSnake.AttackState);
            }

            base.Update();

            if (fireSnake.BattleState.PlayerInAttackRange() && !fireSnake.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                //Debug.Log("Change state");
                StateMachine.ChangeState(fireSnake.MoveState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    

    }
}
