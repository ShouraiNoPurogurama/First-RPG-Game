using UnityEngine;

namespace Enemies.Swordman
{
    //public class EnemySwordmanIdleState : SwordmanGroundedState
    //{
    //    private EnemySwordman _enemySwordman;

    //    public EnemySwordmanIdleState(EnemySwordman enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySwordman swordman)
    //        : base(enemy, stateMachine, animBoolName, swordman)
    //    {
    //    }

    //    public override void Enter()
    //    {
    //        base.Enter();
    //    }

    //    public override void Update()
    //    {
    //        base.Update();
    //    }

    //    public override void Exit()
    //    {
    //        base.Exit();
    //    }
    //}
    public class EnemySwordmanIdleState : EnemySwordmanGroundedState
    {
        public EnemySwordmanIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySwordman enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = enemy.idleTime;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            if (StateTimer<0) 
            {
                StateMachine.ChangeState(enemy.MoveState);
            }
 
        }

    }
}
