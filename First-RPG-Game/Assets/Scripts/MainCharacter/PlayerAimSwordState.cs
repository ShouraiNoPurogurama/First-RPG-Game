using UnityEngine;

namespace MainCharacter
{
    public class PlayerAimSwordState : PlayerState
    {
        private readonly Camera _camera;

        public PlayerAimSwordState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine,
            player, animationBoolName)
        {
            _camera = Camera.main;
        }

        public override void Enter()
        {
            base.Enter();
            
            Player.SkillManager.Sword.SetDotsActive(true);
        }

        public override void Update()
        {
            base.Update();
            
            Player.SetZeroVelocity();

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                StateMachine.ChangeState(Player.IdleState);
            }

            Vector2 mousePosition = _camera!.ScreenToWorldPoint(Input.mousePosition);

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
            Player.StartCoroutine("BusyFor", .15f);
        }
    }
}