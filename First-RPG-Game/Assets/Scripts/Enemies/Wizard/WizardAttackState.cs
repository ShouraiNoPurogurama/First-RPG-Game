using Stats;
using UnityEngine;

namespace Enemies.Wizard
{
    public class WizardAttackState : EnemyState
    {
        private EnemyWizard _wizard;
        private bool hasDealtDamage;

        public WizardAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyWizard wizard)
            : base(enemyBase, stateMachine, animBoolName)
        {
            _wizard = wizard;
        }

        public override void Enter()
        {
            base.Enter();
            hasDealtDamage = false;
        }

        public override void Update()
        {
            base.Update();

            _wizard.SetZeroVelocity();

            if (TriggerCalled)
            {
                if (!hasDealtDamage)
                {
                    hasDealtDamage = true;
                    DealDamage();
                }

                TriggerCalled = false;
                _wizard.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_wizard.BattleState);
            }
        }

        private void DealDamage()
        {
            float attackRadius = 1.5f;
            Collider2D[] hits = Physics2D.OverlapCircleAll(_wizard.transform.position, attackRadius);

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    PlayerStats player = hit.GetComponent<PlayerStats>();
                    if (player != null)
                    {
                        player.TakeDamage(_wizard.CurrentDamage);
                    }
                }
                else if (hit.CompareTag("Enemy"))
                {
                    EnemyWizard enemy = hit.GetComponent<EnemyWizard>();
                    if (enemy != null)
                    {
                        enemy.OnTakeHit();
                    }
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}