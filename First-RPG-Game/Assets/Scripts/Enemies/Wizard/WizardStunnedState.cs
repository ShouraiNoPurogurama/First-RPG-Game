using UnityEngine;

namespace Enemies.Wizard
{
    public class WizardStunnedState : EnemyState
    {
        private EnemyWizard _wizard;

        public WizardStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyWizard wizard) : base(enemyBase, stateMachine, animBoolName)
        {
            _wizard = wizard;
        }

        public override void Enter()
        {
            base.Enter();

            _wizard.FX.InvokeRepeating("RedColorBlink", 0, .1f);
            StateTimer = _wizard.stunDuration;
            Rb.linearVelocity = new Vector2(-_wizard.FacingDir * _wizard.stunDirection.x, _wizard.stunDirection.y);
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
                StateMachine.ChangeState(_wizard.IdleState);
        }

        public override void Exit()
        {
            base.Exit();
            _wizard.FX.Invoke("CancelColorChange", 0);
        }
    }
}