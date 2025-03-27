using UnityEngine;

namespace Enemies.Wizard
{
    public class EnemyWizardIdleState : EnemyWizardGroundedState
    {
        public EnemyWizardIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyWizard enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = enemy.idleTime;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            if (StateTimer < 0)
            {
                //StateMachine.ChangeState(enemy.MoveState);
            }

        }

    }
}
