using UnityEngine;

namespace Enemies.Frogger
{
    public class FroggerJumpState : EnemyState
    {
        private EnemyFrogger _frogger;

        public FroggerJumpState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFrogger enemy) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _frogger = enemy;
        }

        public override void Enter()
        {
            base.Enter();
            Rb.linearVelocity = new Vector2(_frogger.jumpVelocity.x * -_frogger.FacingDir, _frogger.jumpVelocity.y);
        }

        public override void Update()
        {
            base.Update();
            
            // _frogger.Animator.SetFloat("yVelocity", _frogger.Rb.linearVelocity.y);
            
            if(_frogger.IsGroundDetected() && _frogger.Rb.linearVelocity.y <= 0)
            { 
                StateMachine.ChangeState(_frogger.RegenState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}