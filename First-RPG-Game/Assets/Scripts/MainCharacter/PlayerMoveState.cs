namespace MainCharacter
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        
            if (xInput == 0 || Player.IsWallDetected())
            {
                StateMachine.ChangeState(Player.IdleState);
                Player.SetVelocity(0, Rb.linearVelocity.y);
            }
        
            Player.SetVelocity(xInput * Player.moveSpeed, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
