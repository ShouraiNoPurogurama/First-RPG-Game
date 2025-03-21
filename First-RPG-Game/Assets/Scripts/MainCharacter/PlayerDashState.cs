using UnityEngine;

namespace MainCharacter
{
    public class PlayerDashState : PlayerState
    {
        private bool _activateAttack;
        private float _originalStateTimer;

        public PlayerDashState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Player.SkillManager.Clone.CreateCloneOfDashStart();

            StateTimer = Player.dashDuration;

            _originalStateTimer = StateTimer;
        }

        public override void Update()
        {
            base.Update();

            if (Player.IsWallDetected() && !Player.IsGroundDetected() && StateTimer / Player.dashDuration < 0.90)
            {
                StateMachine.ChangeState(Player.WallSlideState);
            }

            float xVelocity = Player.dashSpeed * Player.DashDir;

            if (StateTimer >= 0 && StateTimer < 0.6 * _originalStateTimer)
            {
                xVelocity *= 1.2f * StateTimer / _originalStateTimer;
            }

            Player.SetVelocity(xVelocity, 0);

            if (Input.GetKey(KeyCode.Mouse0))
            {
                _activateAttack = true;
            }

            if (_activateAttack && StateTimer <= .08f)
            {
                Player.isDashAttack = true;
                _activateAttack = false;
                StateMachine.ChangeState(Player.DashAttackState);
            }

            else if (StateTimer <= 0)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();

            Player.SkillManager.Clone.CreateCloneOnDashOver();

            Player.SetVelocity(0, Rb.linearVelocity.y);
        }
    }
}
