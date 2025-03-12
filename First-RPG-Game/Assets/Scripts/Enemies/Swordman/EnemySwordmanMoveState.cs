using Enemies.Swordman;
using UnityEngine;
namespace Enemies.Swordman
{
    public class EnemySwordmanMoveState : EnemyState
    {
        private EnemySwordman enemy;

        public EnemySwordmanMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySwordman _enemy) : base(enemyBase, stateMachine, animBoolName)
        {
            this.enemy = _enemy;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            enemy.SetVelocity(enemy.moveSpeed* enemy.FacingDir, enemy.Rb.linearVelocityY);
            if(enemy.IsWallDetected()|| !enemy.IsGroundDetected())
            {
                enemy.Flip();
                StateMachine.ChangeState(enemy.IdleState);
            }
        }

        //public EnemySwordmanMoveState(EnemySwordman enemy, EnemyStateMachine stateMachine, string animBoolName)
        //    : base(enemy, stateMachine, animBoolName)
        //{
        //    _enemySwordman = enemy;
        //}

        //public override void Update()
        //{
        //    base.Update();

        //    if (_enemySwordman.IsPlayerDetected())
        //    {
        //        _enemySwordman.transform.position = Vector2.MoveTowards(
        //            _enemySwordman.transform.position,
        //            _enemySwordman.IsPlayerDetected().transform.position,
        //            _enemySwordman.moveSpeed * Time.deltaTime);
        //    }
        //}
    }
}

