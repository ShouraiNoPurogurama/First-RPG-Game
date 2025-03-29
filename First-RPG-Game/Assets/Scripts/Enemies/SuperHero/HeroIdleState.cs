using Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HeroIdleState : HeroGroundedState
{
    public HeroIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hero Hero) : base(enemyBase, stateMachine, animBoolName, Hero)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StateTimer = Hero.idleTime;
    }

    public override void Update()
    {
        base.Update();

        //if (StateTimer <= 0)
        //{
        //    StateMachine.ChangeState(Hero.MoveState);
        //}
    }

    public override void Exit()
    {
        base.Exit();
    }
}
