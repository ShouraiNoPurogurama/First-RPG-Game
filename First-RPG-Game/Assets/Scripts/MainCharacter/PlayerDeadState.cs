namespace MainCharacter
{
    public class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            
            Player.SetZeroVelocity();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
