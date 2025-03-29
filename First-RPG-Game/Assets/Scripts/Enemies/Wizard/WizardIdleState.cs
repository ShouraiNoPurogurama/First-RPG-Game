namespace Enemies.Wizard
{
    public class WizardIdleState : WizardGroundedState
    {
        public WizardIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyWizard wizard) :
            base(enemyBase, stateMachine, animBoolName, wizard)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = Wizard.idleTime;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(Wizard.MoveState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}