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

            base.Update();

            fireSpider.SetZeroVelocity();

            if (TriggerCalled)
            {
                TriggerCalled = false;
                fireSpider.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(fireSpider.BattleState);
            }

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
