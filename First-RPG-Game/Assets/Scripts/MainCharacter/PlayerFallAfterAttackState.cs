
namespace MainCharacter
{
    public class PlayerFallAfterAttackState : PlayerAirState
    {
        public PlayerFallAfterAttackState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.Rb.gravityScale *= 2.5f;
        }

        public override void Update()
        {
            base.Update();

            if (Player.IsGroundDetected())
            {
                StateMachine.ChangeState(Player.LandingAttackState);
            }
        }

        public override void Exit()
        {
            Player.Rb.gravityScale /= 2.5f;
            base.Exit();
        }
    }
}
