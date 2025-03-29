namespace Enemies.Wizard
{
    public class WizardMoveState : WizardGroundedState
    {
        public WizardMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyWizard wizard) : base(enemyBase, stateMachine, animBoolName, wizard)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            Wizard.SetVelocity(Wizard.FacingDir * Wizard.moveSpeed, Rb.linearVelocity.y);

            if (!Wizard.IsBusy && (Wizard.IsWallDetected() || !Wizard.IsGroundDetected()))
            {
                Wizard.Flip();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}