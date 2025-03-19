using Enemies.Swordman;
using UnityEngine;

namespace Enemies.Swordman
{
    public class EnemySwordmanAttackState : EnemyState
    {
        EnemySwordman enemy;
        public EnemySwordmanAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySwordman _enemy) : base(enemyBase, stateMachine, animBoolName )
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
        }

        public override void Exit()
        {
            base.Exit();
        }


    }
}
