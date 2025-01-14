using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StateMachine.ChangeState(Player.DashState);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && Player.IsGroundDetected())
        {
            StateMachine.ChangeState(Player.JumpState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
