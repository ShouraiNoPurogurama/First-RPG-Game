using System;
using UnityEngine;

namespace Enemies.FireMage
{
    public class FireMageAttackState : EnemyState
    {
        private EnemyFireMage fireMage;
        public FireMageAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMage _fireMage) : base(enemyBase, stateMachine, animBoolName)
        {
            fireMage = _fireMage;
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            /*
            try
            {
                base.Update();

                fireMage.SetZeroVelocity();

                if (TriggerCalled)
                {
                    TriggerCalled = false;
                    fireMage.lastTimeAttacked = Time.time;
                    StateMachine.ChangeState(fireMage.BattleState);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception in Update(): " + ex.Message);
            }*/
            base.Update();

            fireMage.SetZeroVelocity();

            if (TriggerCalled)
            {
                TriggerCalled = false;
                fireMage.lastTimeAttacked = Time.time;
                fireMage.SpawnFireballs();
                StateMachine.ChangeState(fireMage.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
