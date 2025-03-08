using UnityEngine;

namespace MainCharacter
{
    public class PlayerDashAttackState : PlayerPrimaryAttackState
    {
        private float _flyUpTime = 0.3f;
        private float _elapsedTime;

        public PlayerDashAttackState(PlayerStateMachine stateMachine, Player player, string animationBoolName)
            : base(stateMachine, player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _elapsedTime = 0f;
        }

        public override void Update()
        {
            base.Update();

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime <= _flyUpTime)
            {
                //Initial quick fly-up phase
                float newY = Mathf.MoveTowards(Player.transform.position.y, Player.transform.position.y + 10,
                    10f * Time.deltaTime);
                Player.transform.position = new Vector2(Player.transform.position.x, newY);
            }
            else
            {
                StateMachine.ChangeState(Player.FallAfterAttackState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}