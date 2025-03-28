namespace Enemies.DarkBoss
{
    public class DarkBossMoveState : DarkBossGroundedState
    {
        public DarkBossMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animationBoolName, DarkBoss darkBoss) : base(enemy, stateMachine, animationBoolName, darkBoss)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = DarkBoss.moveCooldown;
        }

        public override void Update()
        {
            base.Update();
            DarkBoss.SetVelocity(DarkBoss.FacingDir * DarkBoss.moveSpeed, Rb.linearVelocity.y);

            if (!DarkBoss.IsBusy && (DarkBoss.IsWallDetected() || !DarkBoss.IsGroundDetected()))
            {
                DarkBoss.Flip();
            }
            if(StateTimer <= 0)
            {
                StateMachine.ChangeState(DarkBoss.IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            
        }
    }
}
