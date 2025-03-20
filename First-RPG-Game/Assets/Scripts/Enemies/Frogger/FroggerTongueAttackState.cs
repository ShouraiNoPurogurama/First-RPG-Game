using Enemies.Frogger;
using UnityEngine;

namespace Enemies.Frogger
{
    public class FroggerTongueAttackState : EnemyState
    {
        private EnemyFrogger _Frogger;

        public FroggerTongueAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFrogger enemyFrogger) : base(enemyBase, stateMachine, animBoolName)
        {
            _Frogger = enemyFrogger;
        }

        public override void Enter()
        {
            base.Enter();            
            _Frogger.SetZeroVelocity();
        }

        public override void Update()
        {
            base.Update();
            
            if (TriggerCalled)
            {
                TriggerCalled = false;
                _Frogger.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_Frogger.BattleState);
            }
        }
    }
}