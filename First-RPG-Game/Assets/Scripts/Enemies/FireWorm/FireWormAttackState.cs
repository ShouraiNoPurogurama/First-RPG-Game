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

            base.Update();
            fireWorm.SetZeroVelocity();
            if (TriggerCalled)
            {
                TriggerCalled = false;
                fireWorm.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(fireWorm.BattleState);
            }
        }

        public override void Exit()
        {
            //Debug.Log("atking 333333333333");
            base.Exit();
        }
    }
}
