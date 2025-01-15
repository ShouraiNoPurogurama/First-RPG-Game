using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine,
        player, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        Player.CheckForDashInput(-Player.FacingDir);
        
        if (xInput != 0 && !Mathf.Approximately(Player.FacingDir, xInput))
        {
            Player.SetVelocity(xInput, Rb.linearVelocity.y);
            StateMachine.ChangeState(Player.AirState);
        }

        if (yInput != 0)
        {
            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, yInput * 1.5f);
        }
        else
        {
            Rb.linearVelocity = new Vector2(0, Rb.linearVelocity.y * 0.7f);
        }

        if (Player.IsGroundDetected())
        {
            StateMachine.ChangeState(Player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}