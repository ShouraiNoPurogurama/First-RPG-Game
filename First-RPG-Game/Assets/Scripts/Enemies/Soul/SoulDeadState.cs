using Enemies.Archer;
using UnityEngine;

namespace Enemies.Soul
{
    public class SoulDeadState : EnemyState
    {
        private EnemySoul _soul;

        public SoulDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySoul enemy) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _soul = enemy;
        }

        public override void Enter()
        {
            base.Enter();
            
        }

        public override void Update()
        {
            base.Update();
            if (TriggerCalled)
            {
                _soul.SelfDestroy();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}