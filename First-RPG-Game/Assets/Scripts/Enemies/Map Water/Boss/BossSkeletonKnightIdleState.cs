namespace Enemies.Map_Water.Boss
{
    public class BossSkeletonKnightIdleState : EnemyState
    {
        private BossSkeletonKnight enemy;

        public BossSkeletonKnightIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, BossSkeletonKnight enemy) : base(enemyBase, stateMachine, animBoolName)
        {
            this.enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            StateTimer = enemy.idleTime;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(enemy.MoveState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
