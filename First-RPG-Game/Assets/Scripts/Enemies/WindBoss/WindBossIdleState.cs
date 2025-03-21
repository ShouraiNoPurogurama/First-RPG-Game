namespace Enemies.WindBoss
{
    public class WindBossIdleState : WindBossGroundedState
    {
        public WindBossIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) : base(
            enemyBase, stateMachine, animBoolName, windBoss)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StateTimer = WindBoss.idleTime;
        }

        public override void Update()
        {
            
            WindBoss.SetZeroVelocity();

            base.Update();
            
            if (WindBoss.BattleState.PlayerInAttackRange() && !WindBoss.BattleState.CanAttack())
            {
                return;
            }
            
            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(WindBoss.MoveState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}