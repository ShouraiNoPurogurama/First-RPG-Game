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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StateMachine.ChangeState(Player.PrimaryAttack);
        }
        
        if (!Player.IsGroundDetected())
        {
            StateMachine.ChangeState(Player.AirState);
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
