using System;
using UnityEngine;

namespace Enemies.FireMiniMage
{
    public class FireMiniMageAttackState : EnemyState
    {
        private EnemyFireMiniMage FireMiniMage;
        public FireMiniMageAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMiniMage _FireMiniMage) : base(enemyBase, stateMachine, animBoolName)
        {
            FireMiniMage = _FireMiniMage;
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            
            try
            {
                base.Update();

                FireMiniMage.SetZeroVelocity();

                if (TriggerCalled)
                {
                    TriggerCalled = false;
                    FireMiniMage.lastTimeAttacked = Time.time;
                    StateMachine.ChangeState(FireMiniMage.BattleState);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception in Update(): " + ex.Message);
            }
            /*base.Update();

            FireMiniMage.SetZeroVelocity();

            if (TriggerCalled)
            {
                TriggerCalled = false;
                FireMiniMage.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(FireMiniMage.BattleState);
            }*/
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
