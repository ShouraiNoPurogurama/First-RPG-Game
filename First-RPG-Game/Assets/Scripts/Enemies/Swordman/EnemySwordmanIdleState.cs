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
    public class EnemySwordmanIdleState : EnemyState
    {
        private EnemySwordman enemy;
        public  EnemySwordmanIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySwordman _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
        {
            enemy = _enemy;
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
