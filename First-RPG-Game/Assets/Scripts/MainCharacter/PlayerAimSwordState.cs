using UnityEngine;

namespace MainCharacter
{
    public class PlayerAimSwordState : PlayerState
    {
        public PlayerAimSwordState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKeyUp(KeyCode.Mouse1))
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
