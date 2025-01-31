namespace MainCharacter
{
    public class PlayerStateMachine
    {
        public PlayerState CurrentState { get; private set; }

        /// <summary>
        /// Enter the state machine
        /// </summary>
        /// <param name="state">The state to be initialized</param>
        public void Initialize(PlayerState state)
        {
            CurrentState = state;
            CurrentState.Enter();
        }

        public void ChangeState(PlayerState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}