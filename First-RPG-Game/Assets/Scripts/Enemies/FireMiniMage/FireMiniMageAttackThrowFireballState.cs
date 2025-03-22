using Enemies;
using Enemies.FireMiniMage;
using UnityEngine;

namespace Enemies.FireMiniMage
{
    public class FireMiniMageAttackThrowFireballState : EnemyState
    {
        private EnemyFireMiniMage _miniMage;

        public FireMiniMageAttackThrowFireballState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMiniMage enemyFireMiniMage) : base(enemyBase, stateMachine, animBoolName)
        {
            _miniMage = enemyFireMiniMage;
        }

        public override void Update()
        {
            base.Update();

            if (TriggerCalled)
            {
                TriggerCalled = false;
                _miniMage.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_miniMage.BattleState);
            }
        }
    }
}
