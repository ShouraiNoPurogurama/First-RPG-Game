using UnityEngine;

namespace Enemies.Frogger
{
    public class FroggerSpitAttackState : EnemyState
    {
        private EnemyFrogger _frogger;

        public FroggerSpitAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFrogger enemy) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _frogger = enemy;
        }

        public override void Enter()
        {
            base.Enter();
            _frogger.lastTimeSpit = Time.time;
            _frogger.lastTimeAttacked = Time.time;
            _frogger.SetZeroVelocity();
        }

        public override void Update()
        {
            base.Update();
            
            if (TriggerCalled)
            {
                TriggerCalled = false;
                _frogger.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_frogger.BattleState);
            }
        }
        

        public override void Exit()
        {
            base.Exit();
        }
    }
}