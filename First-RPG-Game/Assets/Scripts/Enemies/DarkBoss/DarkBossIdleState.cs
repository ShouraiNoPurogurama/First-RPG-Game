namespace Enemies.DarkBoss
{
    public class DarkBossIdleState : DarkBossGroundedState
    {
        public DarkBossIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animationBoolName, DarkBoss darkBoss) : base(enemy, stateMachine, animationBoolName, darkBoss)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = DarkBoss.idleTime;
        }
        public override void Update()
        {
            
            if (DarkBoss.BattleState.PlayerInAttackRange() && DarkBoss.BattleState.CanAttack())
            {
                StateMachine.ChangeState(DarkBoss.AttackState);
            }
            base.Update();

            if (DarkBoss.BattleState.PlayerInAttackRange() && !DarkBoss.BattleState.CanAttack())
            {
                return;
            }
            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(DarkBoss.MoveState);
            }
        }
        public override void Exit() { base.Exit(); }

    }
}
