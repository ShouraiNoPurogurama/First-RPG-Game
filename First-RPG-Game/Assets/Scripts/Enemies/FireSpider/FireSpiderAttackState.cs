using System;
using UnityEngine;

namespace Enemies.FireSpider
{
    public class FireSpiderAttackState : EnemyState
    {
        private EnemyFireSpider fireSpider;
        public FireSpiderAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSpider _fireSpider) : base(enemyBase, stateMachine, animBoolName)
        {
            fireSpider = _fireSpider;
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

                fireSpider.SetZeroVelocity();

                if (TriggerCalled)
                {
                    TriggerCalled = false;
                    fireSpider.lastTimeAttacked = Time.time;
                    StateMachine.ChangeState(fireSpider.BattleState);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception in Update(): " + ex.Message);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
