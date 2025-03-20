namespace Enemies.Frogger
{
    public class FroggerIdleState : FroggerGroundedState
    {
        public FroggerIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName,
            EnemyFrogger frogger) : base(
            enemyBase, stateMachine, animBoolName, frogger)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StateTimer = Frogger.idleTime;
        }

        public override void Update()
        {
            Frogger.SetZeroVelocity();

            if (Frogger.BattleState.PlayerInAttackRange() && Frogger.BattleState.CanAttack())
            {
                StateMachine.ChangeState(Frogger.TongueAttackState);
            }

            base.Update();

            if (Frogger.BattleState.PlayerInAttackRange() && !Frogger.BattleState.CanAttack())
            {
                return;
            }

            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(Frogger.MoveState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}