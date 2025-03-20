using UnityEngine;

namespace Enemies.Archer
{
    public class ArcherIdleState : ArcherGroundedState
    {
        public ArcherIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher archer) : base(
            enemyBase, stateMachine, animBoolName, archer)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StateTimer = Archer.idleTime;
        }

        public override void Update()
        {
            
            Archer.SetZeroVelocity();
            
            if (Archer.BattleState.PlayerInAttackRange() && Archer.BattleState.CanAttack())
            {
                StateMachine.ChangeState(Archer.AttackState);
            }

            base.Update();
            
            if (Archer.BattleState.PlayerInAttackRange() && !Archer.BattleState.CanAttack())
            {
                return;
            }
            
            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(Archer.MoveState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}