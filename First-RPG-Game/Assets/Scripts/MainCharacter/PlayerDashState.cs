namespace MainCharacter
{
    public class PlayerDashState : PlayerState
    {
        public PlayerDashState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StateTimer = Player.dashDuration;
        
        }

        public override void Update()
        {
            base.Update();

            Player.SetVelocity( Player.dashSpeed * Player.DashDir , 0);
        
            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        
            Player.SetVelocity(0, Rb.linearVelocity.y);
        }
    }
}
