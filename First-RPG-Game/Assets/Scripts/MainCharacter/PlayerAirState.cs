namespace MainCharacter
{
    public class PlayerAirState : PlayerState
    {
        public PlayerAirState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player,
            animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            if (Player.IsWallDetected())
            {
                StateMachine.ChangeState(Player.WallSlideState);
            }
        
            if (Player.IsGroundDetected())
            {
                StateMachine.ChangeState(Player.IdleState);
            }

            if (xInput != 0)
            {
                Player.SetVelocity(0.8f * xInput  * Player.moveSpeed, Rb.linearVelocity.y);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}