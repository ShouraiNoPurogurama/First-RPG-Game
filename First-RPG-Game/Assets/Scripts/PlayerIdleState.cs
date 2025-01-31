using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player,
        animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        Player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();
        if (xInput != 0 && !Player.IsBusy && (!Player.IsWallDetected() ||
                            Player.IsWallDetected() && !Mathf.Approximately(xInput, Player.FacingDir)))
        {
            StateMachine.ChangeState(Player.MoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}