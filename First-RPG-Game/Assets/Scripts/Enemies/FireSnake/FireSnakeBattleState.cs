using MainCharacter;
using UnityEngine;

namespace Enemies.FireSnake
{
    public class FireSnakeBattleState : EnemyState
    {
        private EnemyFireSnake fireSnake;
        private Transform _player;
        private int _moveDir;

        public FireSnakeBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSnake _fireSnake) :
            base(enemyBase, stateMachine, animBoolName)
        {
            fireSnake = _fireSnake;
        }

        public override void Enter()
        {
            //Debug.Log("Im in battle state");
            base.Enter();

            // _player = GameObject.Find("Player").transform;
            AttachCurrentPlayerIfNotExists();

            //if player in attack range, block FireSnake movement
            if (PlayerInAttackRange() && !CanAttack())
            {
                fireSnake.SetZeroVelocity();
                StateMachine.ChangeState(fireSnake.IdleState);
            }
        }

        public override void Update()
        {
            base.Update();

            if (fireSnake.IsPlayerDetected())
            {
                StateTimer = fireSnake.battleTime;

                if (fireSnake.IsGroundDetected() && fireSnake.IsPlayerDetected().distance <= fireSnake.attackDistance && CanAttack())
                {
                    StateMachine.ChangeState(fireSnake.AttackState);
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, fireSnake.transform.position) > 10)
                {
                    //Debug.Log("Attack2");
                    StateMachine.ChangeState(fireSnake.IdleState);
                }
            }

            _moveDir = _player.position.x > fireSnake.transform.position.x ? 1 : -1;
            //Debug.Log(_moveDir);

            //if player in attack range, block FireSnake movement
            if (PlayerInAttackRange() && CanAttack())
            {
                fireSnake.SetZeroVelocity();
                StateMachine.ChangeState(fireSnake.IdleState);
                //return;
            }
            if (fireSnake.IsWallDetected())
            {
                fireSnake.Flip();
                fireSnake.SetVelocity(fireSnake.moveSpeed * _moveDir, Rb.linearVelocity.y);
            }
            else
                fireSnake.SetVelocity(fireSnake.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(fireSnake.lastTimeAttacked, 0) || Time.time >= fireSnake.lastTimeAttacked + fireSnake.attackCooldown)
            {
                // _fireSnake.lastTimeAttacked = Time.time;
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = fireSnake.IsPlayerDetected().distance != 0 &&
             fireSnake.IsPlayerDetected().distance <= fireSnake.attackDistance &&
             (fireSnake.FacingDir == -1 && _player.transform.position.x <= fireSnake.transform.position.x ||
              fireSnake.FacingDir == 1 && _player.transform.position.x >= fireSnake.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - fireSnake.transform.position.x) < fireSnake.attackDistance &&
                Mathf.Abs(_player.transform.position.y - fireSnake.transform.position.y) <=
                fireSnake.CapsuleCollider.bounds.size.y)
            {
                result = true;
            }
            return result;
        }

        private void AttachCurrentPlayerIfNotExists()
        {
            if (!_player)
            {
                _player = PlayerManager.Instance.player.transform;
            }
        }

    }
}
