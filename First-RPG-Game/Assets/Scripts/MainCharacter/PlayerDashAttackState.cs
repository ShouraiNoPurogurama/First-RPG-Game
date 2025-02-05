namespace MainCharacter
{
    public class PlayerDashAttackState : PlayerPrimaryAttackState
    {
        public PlayerDashAttackState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
