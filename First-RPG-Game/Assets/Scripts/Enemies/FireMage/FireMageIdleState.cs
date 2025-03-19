using UnityEngine;

namespace Enemies.FireMage
{
    public class FireMageIdleState : FireMageGroundedState
    {
        public FireMageIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMage _fireMage) : base(enemyBase, stateMachine, animBoolName, _fireMage)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = fireMage.idleTime;
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
                StateMachine.ChangeState(fireMage.MoveState);
            }
         
            /*
            if (fireMage.BattleState.PlayerInAttackRange() && fireMage.BattleState.CanAttack())
            {
                StateMachine.ChangeState(fireMage.TongueAttackState);
            }

            base.Update();

            if (fireMage.BattleState.PlayerInAttackRange() && !fireMage.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                Debug.Log("Change state");
                StateMachine.ChangeState(fireMage.MoveState);
            }
        }*/

        public override void Update()
        {
            //Debug.Log("player: " + fireMage.BattleState.PlayerInAttackRange());
            //Debug.Log("atk" + fireMage.BattleState.CanAttack());
            if (fireMage.BattleState.PlayerInAttackRange() && fireMage.BattleState.CanAttack())
            {
                //Debug.Log("attacking dsdsđsdsd");
                StateMachine.ChangeState(fireMage.AttackState);
            }

            base.Update();

            if (fireMage.BattleState.PlayerInAttackRange() && !fireMage.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                //Debug.Log("Change state");
                StateMachine.ChangeState(fireMage.MoveState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    

    }
}
