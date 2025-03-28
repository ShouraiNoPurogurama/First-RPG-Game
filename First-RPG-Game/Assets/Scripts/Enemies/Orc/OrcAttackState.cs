using Enemies;
using Enemies.Orc;
using UnityEngine;

namespace Enemies.Orc
{
    public class OrcAttackState : EnemyState
    {
        private Orc Orc;
        public OrcAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Orc orc) : base(enemyBase, stateMachine, animBoolName)
        {
            Orc = orc;
        }
        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            Orc.SetZeroVelocity();

            if (TriggerCalled)
            {
                //SoundManager.PlaySFX("Enemy", 0);
                TriggerCalled = false;
                Orc.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(Orc.battleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

