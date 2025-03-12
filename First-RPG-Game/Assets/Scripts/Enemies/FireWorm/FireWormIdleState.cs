using UnityEngine;

namespace Enemies.FireWorm
{
    public class FireWormIdleState : FireWormGroundedState
    {
        public FireWormIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireWorm _fireWorm) : base(enemyBase, stateMachine, animBoolName, _fireWorm)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = fireWorm.idleTime;
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
                StateMachine.ChangeState(fireWorm.MoveState);
            }
         
            /*
            if (fireWorm.BattleState.PlayerInAttackRange() && fireWorm.BattleState.CanAttack())
            {
                StateMachine.ChangeState(fireWorm.AttackState);
            }

            base.Update();

            if (fireWorm.BattleState.PlayerInAttackRange() && !fireWorm.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                Debug.Log("Change state");
                StateMachine.ChangeState(fireWorm.MoveState);
            }
        }*/

        public override void Update()
        {
            //Debug.Log("player: " + fireWorm.BattleState.PlayerInAttackRange());
            //Debug.Log("atk" + fireWorm.BattleState.CanAttack());
            if (fireWorm.BattleState.PlayerInAttackRange() && fireWorm.BattleState.CanAttack())
            {
                //Debug.Log("attacking dsdsđsdsd");
                StateMachine.ChangeState(fireWorm.AttackState);
            }

            base.Update();

            if (fireWorm.BattleState.PlayerInAttackRange() && !fireWorm.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                //Debug.Log("Change state");
                StateMachine.ChangeState(fireWorm.MoveState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    

    }
}
