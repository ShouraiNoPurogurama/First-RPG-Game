using UnityEngine;

namespace Enemies.Boss
{
    public class BossIdleState : BossGroundedState
    {
        public BossIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyBoss _boss) : base(enemyBase, stateMachine, animBoolName, _boss)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = boss.idleTime;
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
                StateMachine.ChangeState(boss.MoveState);
            }
         
            /*
            if (boss.BattleState.PlayerInAttackRange() && boss.BattleState.CanAttack())
            {
                StateMachine.ChangeState(boss.TongueAttackState);
            }

            base.Update();

            if (boss.BattleState.PlayerInAttackRange() && !boss.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                Debug.Log("Change state");
                StateMachine.ChangeState(boss.MoveState);
            }
        }*/

        public override void Update()
        {
            //Debug.Log("player: " + boss.BattleState.PlayerInAttackRange());
            //Debug.Log("atk" + boss.BattleState.CanAttack());
            if (boss.BattleState.PlayerInAttackRange() && boss.BattleState.CanAttack())
            {
                Debug.Log("attacking dsdsđsdsd");
                StateMachine.ChangeState(boss.AttackState);
            }

            base.Update();

            if (boss.BattleState.PlayerInAttackRange() && !boss.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                //Debug.Log("Change state");
                StateMachine.ChangeState(boss.MoveState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
}
