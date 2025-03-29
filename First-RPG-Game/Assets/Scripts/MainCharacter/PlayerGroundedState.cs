using Skills;
using Skills.SkillControllers;
using UnityEngine;

namespace MainCharacter
{
    public class PlayerGroundedState : PlayerState
    {
        public PlayerGroundedState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine,
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

            if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoThrownSword())
            {
                StateMachine.ChangeState(Player.AimSwordState);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                StateMachine.ChangeState(Player.CounterAttackState);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StateMachine.ChangeState(Player.CounterWaterAttackState);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                StateMachine.ChangeState(Player.CounterWaterAttackState);
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                StateMachine.ChangeState(Player.PrimaryAttackState);
            }

            if (!Player.IsGroundDetected())
            {
                StateMachine.ChangeState(Player.AirState);
            }

            if (Input.GetKeyDown(KeyCode.Space) && Player.IsGroundDetected())
            {
                StateMachine.ChangeState(Player.JumpState);
            }

            if (Input.GetKeyDown(KeyCode.R) && SkillManager.Instance.BlackHole.CanUseSkillWithNoLogic())
            {
                StateMachine.ChangeState(Player.BlackHoleState);
                Player.FX.CreatePopUpText("Cooldown!",Color.white);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        private bool HasNoThrownSword()
        {
            if (!Player.ThrownSword)
            {
                return true;
            }

            Player.ThrownSword.GetComponent<SwordSkillController>().ReturnSword();

            return false;
        }
    }
}