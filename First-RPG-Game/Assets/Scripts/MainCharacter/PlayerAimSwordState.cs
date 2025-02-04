using UnityEngine;

namespace MainCharacter
{
    public class PlayerAimSwordState : PlayerState
    {
        public PlayerAimSwordState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine,
            player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Player.Skill.Sword.SetDotsActive(true);
        }

        public override void Update()
        {
            base.Update();
            
            // Player.SetZeroVelocity();

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                StateMachine.ChangeState(Player.IdleState);
            }

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Player.transform.position.x > mousePosition.x && Player.FacingDir == 1)
            {
                Player.Flip();
            }
            else if (Player.transform.position.x < mousePosition.x && Player.FacingDir == -1)
            {
                Player.Flip();
            }
        }

        public override void Exit()
        {
            base.Exit();
            Player.StartCoroutine("BusyFor", .2f);
        }
    }
}