using UnityEngine;

namespace Enemies.Archer
{
    public class ArcherJumpState : EnemyState
    {
        private EnemyArcher _archer;

        public ArcherJumpState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher enemy) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _archer = enemy;
        }

        public override void Enter()
        {
            base.Enter();
            Rb.linearVelocity = new Vector2(_archer.jumpVelocity.x * -_archer.FacingDir, _archer.jumpVelocity.y);
            _archer.lastTimeJumped = Time.time;
        }

        public override void Update()
        {
            base.Update();
            
            _archer.Animator.SetFloat("yVelocity", _archer.Rb.linearVelocity.y);
            
            if(_archer.IsGroundDetected() && _archer.Rb.linearVelocity.y <= 0)
            { 
                StateMachine.ChangeState(_archer.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}