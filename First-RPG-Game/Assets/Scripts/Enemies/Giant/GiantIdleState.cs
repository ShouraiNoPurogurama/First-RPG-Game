namespace Enemies.Giant
{
    public class GiantIdleState : GiantGroundedState
    {
        public GiantIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Giant giant) : base(enemyBase, stateMachine, animBoolName, giant)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = Giant.idleTime;
        }
        public override void Update()
        {
            if (Giant.BattleState.PlayerInAttackRange() && Giant.BattleState.CanAttack())
            {
                StateMachine.ChangeState(Giant.AttackState);
            }

            base.Update();

            if (Giant.BattleState.PlayerInAttackRange() && !Giant.BattleState.CanAttack())
            {
                return;
            }
            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(Giant.MoveState);
            }
        }
        public override void Exit() { base.Exit(); }
    }
}
