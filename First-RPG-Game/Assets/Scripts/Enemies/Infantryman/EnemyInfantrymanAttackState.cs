using UnityEngine;

namespace Enemies.Infantryman
{
    public class EnemyInfantrymanAttackState : EnemyState
    {
        private EnemyInfantryman enemy;
        public EnemyInfantrymanAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyInfantryman _enemy) : base(enemyBase, stateMachine, animBoolName )
        {
            this.enemy = _enemy;
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            base.Update();
            enemy.SetZeroVelocity();

            if(TriggerCalled)
            {
                StateMachine.ChangeState(enemy.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            enemy.lastTimeAttacked = Time.time;
        }


    }
}
