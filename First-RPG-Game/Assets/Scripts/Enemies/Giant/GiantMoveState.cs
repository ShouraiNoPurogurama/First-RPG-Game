using Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GiantMoveState : GiantGroundedState
{
    public GiantMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Giant giant) : base(enemyBase, stateMachine, animBoolName, giant)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        Giant.SetVelocity(Giant.FacingDir * Giant.moveSpeed, Rb.linearVelocity.y);

        if (!Giant.IsBusy && (Giant.IsWallDetected() || !Giant.IsGroundDetected()))
        {
            Giant.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
