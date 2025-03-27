using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossJumpState : EnemyState
    {
        private WindBoss _windBoss;

        public WindBossJumpState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }

        public override void Enter()
        {
            base.Enter();
            Rb.linearVelocity = new Vector2(_windBoss.jumpVelocity.x * -_windBoss.FacingDir, _windBoss.jumpVelocity.y);
        }

        public override void Update()
        {
            base.Update();
            
            _windBoss.Animator.SetFloat("yVelocity", _windBoss.Rb.linearVelocity.y);
            
            if(_windBoss.IsGroundDetected() && _windBoss.Rb.linearVelocity.y <= 0)
            { 
                StateMachine.ChangeState(_windBoss.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}