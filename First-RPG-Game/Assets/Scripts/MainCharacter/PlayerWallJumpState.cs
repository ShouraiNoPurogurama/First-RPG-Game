using Audio;

namespace MainCharacter
{
    public class PlayerWallJumpState : PlayerState
    {
        public PlayerWallJumpState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StateTimer = .4f;
            Player.SetVelocity( -Player.FacingDir * Player.moveSpeed * 0.7f, Rb.linearVelocity.y + Player.jumpForce);

            Player.StartCoroutine("BusyFor", .18f);
            SoundManager.PlaySFX("WallJump", 0);
        }

        public override void Update()
        {
            base.Update();

            if (xInput != 0 && !Player.IsBusy)
            {
                Player.SetVelocity(0.8f * xInput * Player.moveSpeed, Rb.linearVelocity.y);
            }
            
            if (StateTimer < 0)
            {
                StateMachine.ChangeState(Player.AirState);
            }
        
        
        }

        public override void Exit()
        {
            base.Exit();
            SoundManager.PlaySFX("WallJump", 1);
        }
    }
}
