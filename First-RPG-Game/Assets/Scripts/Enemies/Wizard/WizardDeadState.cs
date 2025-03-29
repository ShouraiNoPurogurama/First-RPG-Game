using UnityEngine;

namespace Enemies.Wizard
{
    public class WizardDeadState : EnemyState
    {
        private readonly EnemyWizard _wizard;
        
        public WizardDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyWizard wizard) : base(enemyBase, stateMachine, animBoolName)
        {
            _wizard = wizard;
        }

        public override void Enter()
        {
            base.Enter();

            _wizard.Animator.SetBool(_wizard.LastAnimBoolName, true);
            _wizard.Animator.speed = 0;
            _wizard.CapsuleCollider.enabled = false;
            _wizard.transform.position = new Vector3(_wizard.transform.position.x, _wizard.transform.position.y, 10);

            StateTimer = .15f;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer > 0)
            {
                Rb.linearVelocity = new Vector2(0, 10);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}