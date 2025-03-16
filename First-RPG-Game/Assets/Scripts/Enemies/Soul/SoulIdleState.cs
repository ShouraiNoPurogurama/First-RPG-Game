namespace Enemies.Soul
{
    public class SoulIdleState : SoulGroundedState
    {
        public SoulIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySoul soul) : base(
            enemyBase, stateMachine, animBoolName, soul)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StateTimer = Soul.idleTime;
        }

        public override void Update()
        {
            // Soul.SetZeroVelocity();

            base.Update();

            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(Soul.MoveState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}