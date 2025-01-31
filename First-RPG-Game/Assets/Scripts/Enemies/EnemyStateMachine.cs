namespace Enemies
{
    public class EnemyStateMachine
    {
        public EnemyState CurrentState { get; private set; }

        public void Initialize(EnemyState state)
        {
            CurrentState = state;
            CurrentState.Enter();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void ChangeState(EnemyState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}