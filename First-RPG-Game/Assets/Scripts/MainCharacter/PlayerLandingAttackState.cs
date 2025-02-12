namespace MainCharacter
{
    public class PlayerLandingAttackState : PlayerPrimaryAttackState
    {
        public PlayerLandingAttackState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
        {
        }
    }
}
