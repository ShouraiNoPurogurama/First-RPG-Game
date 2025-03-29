using Enemies.Infantryman;
using UnityEngine;
namespace Enemies.Infantryman
{
    public class EnemyInfantrymanMoveState : EnemyInfantrymanGroundedState
    {
        public EnemyInfantrymanMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyInfantryman enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
        {
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
            //Debug.Log("MoveState: Speed=" + enemy.moveSpeed + ", FacingDir=" + enemy.FacingDir);
            enemy.SetVelocity(enemy.moveSpeed* enemy.FacingDir, Rb.linearVelocity.y);
            if (!enemy.IsGroundDetected())
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

