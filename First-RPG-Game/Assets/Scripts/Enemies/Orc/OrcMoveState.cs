namespace Enemies.Orc
{
    public class OrcMoveState : OrcGroundedState
    {
        public OrcMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Orc skeleton) : base(enemyBase, stateMachine, animBoolName, skeleton)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        
            Orc.SetVelocity(Orc.FacingDir * Orc.moveSpeed, Rb.linearVelocity.y);

            if (!Orc.IsBusy && (Orc.IsWallDetected() || !Orc.IsGroundDetected()))
            {
                Orc.Flip();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
