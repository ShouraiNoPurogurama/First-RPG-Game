using System;
using UnityEngine;

namespace Enemies.FireWorm
{
    public class FireWormAttackState : EnemyState
    {
        private EnemyFireWorm fireWorm;
        public FireWormAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireWorm _fireWorm) : base(enemyBase, stateMachine, animBoolName)
        {
            fireWorm = _fireWorm;
        }

        public override void Enter()
        {
            //Debug.Log("atking .................");
            base.Enter();
            //Debug.Log("atking 111111111");
        }
        public override void Update()
        {
            try
            {
                //Debug.Log("atking 2222222222");
                base.Update();

                //Debug.Log("Check before 44444444444");

                //Debug.Log("atking 44444444444");

                fireWorm.SetZeroVelocity();
                //Debug.Log("atking 55555555             " + TriggerCalled);
                if (TriggerCalled)
                {
                    //Debug.Log("atking 666666666");
                    TriggerCalled = false;
                    fireWorm.lastTimeAttacked = Time.time;
                    StateMachine.ChangeState(fireWorm.BattleState);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception in Update(): " + ex.Message);
            }
        }

        /*
        public override void Update()
        {
            Debug.Log("atking 2222222222");

            base.Update();

            if (StateMachine.CurrentState != this)
            {
                Debug.Log("State changed before logging 44444444444!");
                return;
            }

            Debug.Log("atking 44444444444");
            fireWorm.SetZeroVelocity();

            if (TriggerCalled)
            {
                Debug.Log("atking");
                TriggerCalled = false;
                fireWorm.lastTimeAttacked = Time.time;
                //StateMachine.ChangeState(fireWorm.BattleState);
            }
        }*/

        public override void Exit()
        {
            //Debug.Log("atking 333333333333");
            base.Exit();
        }
    }
}
