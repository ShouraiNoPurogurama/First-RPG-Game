using Manager_Controller;
using UnityEngine;

namespace MainCharacter
{
    public class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
        {
        }
        /// <summary>
        /// Enter the state and switch on the darkScreen
        /// </summary>
        public override void Enter()
        {
            base.Enter();
            GameManager.Instance.isDied = true;
            GameObject.Find("Canvas").GetComponent<UI.UI>().SwitchOnEndScreen();
            Debug.Log("Died");
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
