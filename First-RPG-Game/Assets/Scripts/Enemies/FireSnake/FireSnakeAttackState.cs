using System;
using UnityEngine;

namespace Enemies.FireSnake
{
    public class FireSnakeAttackState : EnemyState
    {
        private EnemyFireSnake fireSnake;
        public FireSnakeAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSnake _fireSnake) : base(enemyBase, stateMachine, animBoolName)
        {
            fireSnake = _fireSnake;
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

                fireSnake.SetZeroVelocity();

                if (TriggerCalled)
                {
                    TriggerCalled = false;
                    fireSnake.lastTimeAttacked = Time.time;
                    StateMachine.ChangeState(fireSnake.BattleState);
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
