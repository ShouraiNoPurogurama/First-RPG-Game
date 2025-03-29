using UnityEngine;

namespace Enemies.Wizard
{
    public class WizardGroundedState : EnemyState
    {
        protected readonly EnemyWizard Wizard;
        private Transform _player;

        protected WizardGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyWizard wizard) : base(enemyBase, stateMachine, animBoolName)
        {
            Wizard = wizard;
        }

        public override void Enter()
        {
            base.Enter();
            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();

            if (Wizard.IsPlayerDetected() || Vector2.Distance(Wizard.transform.position, _player.position) < 2)
            {
                StateMachine.ChangeState(Wizard.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}