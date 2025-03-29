using Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HeroMoveState : HeroGroundedState
{
    public HeroMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hero Hero) : base(enemyBase, stateMachine, animBoolName, Hero)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        Hero.SetVelocity(Hero.FacingDir * Hero.moveSpeed, Rb.linearVelocity.y);

        if (!Hero.IsBusy && (Hero.IsWallDetected() || !Hero.IsGroundDetected()))
        {
            Hero.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
