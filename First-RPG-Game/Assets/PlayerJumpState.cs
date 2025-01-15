using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, Player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0)
        {
            Player.SetVelocity(0.8f * xInput, Rb.linearVelocity.y);
        }
        
        if (Rb.linearVelocity.y < 0)
        {
            StateMachine.ChangeState(Player.AirState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}