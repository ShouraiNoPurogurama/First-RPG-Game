using UnityEngine;

namespace MainCharacter
{
    public class PlayerCatchSwordState : PlayerState
    {
        private Transform _sword;
        
        public PlayerCatchSwordState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _sword = Player.ThrownSword.transform;
            
            if (Player.transform.position.x > _sword.position.x && Player.FacingDir == 1)
            {
                Player.Flip();
            }
            else if (Player.transform.position.x < _sword.position.x && Player.FacingDir == -1)
            {
                Player.Flip();
            }

            Rb.linearVelocity = new Vector2(Player.swordReturnImpact * -Player.FacingDir, Rb.linearVelocity.y);
        }

        public override void Update()
        {
            base.Update();

            if (TriggerCalled)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            Player.StartCoroutine("BusyFor", .2f);
        }
    }
}
