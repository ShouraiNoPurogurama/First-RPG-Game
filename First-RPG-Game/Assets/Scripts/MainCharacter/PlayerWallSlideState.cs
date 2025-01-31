using UnityEngine;

namespace MainCharacter
{
    public class PlayerWallSlideState : PlayerState
    {
        private readonly float _wallSlideFallingSpeed = 0.7f;
        private readonly float _wallSlideFallingEnchantedSpeed = 1.5f;
    
        public PlayerWallSlideState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine,
            player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StateMachine.ChangeState(Player.WallJumpState);
                return;
            }
        
            Player.CheckForDashInput(-Player.FacingDir);
        
            if (xInput != 0 && !Mathf.Approximately(Player.FacingDir, xInput))
            {
                Player.SetVelocity(xInput * Player.moveSpeed, Rb.linearVelocity.y);
                StateMachine.ChangeState(Player.AirState);
            }

            if (yInput != 0)
            {
                Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, yInput * _wallSlideFallingEnchantedSpeed);
            }
            else
            {
                Rb.linearVelocity = new Vector2(0, Rb.linearVelocity.y * _wallSlideFallingSpeed);
            }

            if (Player.IsGroundDetected())
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}