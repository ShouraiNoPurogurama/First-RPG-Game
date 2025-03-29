namespace Enemies.Map_Water.Boss
{
    public class BossSkeletonKnightMoveState : BossSkeletonKnightGroundState
    {
        private BossSkeletonKnight enemy;
        public BossSkeletonKnightMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, BossSkeletonKnight enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
        {
            this.enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            enemy.SetVelocity(enemy.FacingDir * enemy.moveSpeed, Rb.linearVelocity.y);

            if (!enemy.IsBusy && (enemy.IsWallDetected() || !enemy.IsGroundDetected()))
            {
                enemy.Flip();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
