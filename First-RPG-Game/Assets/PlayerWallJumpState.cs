public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StateTimer = .4f;
        Player.SetVelocity( -Player.FacingDir, Rb.linearVelocity.y + Player.jumpForce/2);
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer < 0)
        {
            StateMachine.ChangeState(Player.AirState);
        }
        
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
