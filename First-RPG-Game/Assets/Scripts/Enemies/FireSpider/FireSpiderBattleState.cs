using MainCharacter;
using UnityEngine;

namespace Enemies.FireSpider
{
    public class FireSpiderBattleState : EnemyState
    {
        private EnemyFireSpider fireSpider;
        private Transform _player;
        private int _moveDir;

        public FireSpiderBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSpider _fireSpider) :
            base(enemyBase, stateMachine, animBoolName)
        {
            fireSpider = _fireSpider;
        }

        public override void Enter()
        {
            //Debug.Log("Im in battle state");
            base.Enter();

            // _player = GameObject.Find("Player").transform;
            AttachCurrentPlayerIfNotExists();

            //if player in attack range, block FireSpider movement
            if (PlayerInAttackRange() && !CanAttack())
            {
                fireSpider.SetZeroVelocity();
                StateMachine.ChangeState(fireSpider.IdleState);
            }
        }

        public override void Update()
        {
            base.Update();

            if (fireSpider.IsPlayerDetected())
            {
                StateTimer = fireSpider.battleTime;

                if (fireSpider.IsGroundDetected() && fireSpider.IsPlayerDetected().distance <= fireSpider.attackDistance && CanAttack())
                {
                    StateMachine.ChangeState(fireSpider.AttackState);
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, fireSpider.transform.position) > 10)
                {
                    //Debug.Log("Attack2");
                    StateMachine.ChangeState(fireSpider.IdleState);
                }
            }

            _moveDir = _player.position.x > fireSpider.transform.position.x ? 1 : -1;
            //Debug.Log(_moveDir);

            //if player in attack range, block FireSpider movement
            if (PlayerInAttackRange())
            {
                fireSpider.SetZeroVelocity();
                StateMachine.ChangeState(fireSpider.IdleState);
                //return;
            }

            if (fireSpider.IsWallDetected())
            {
                fireSpider.Flip();
                fireSpider.SetVelocity(fireSpider.moveSpeed * _moveDir, Rb.linearVelocity.y);
            }
            else
                fireSpider.SetVelocity(fireSpider.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(fireSpider.lastTimeAttacked, 0) || Time.time >= fireSpider.lastTimeAttacked + fireSpider.attackCooldown)
            {
                // _fireSpider.lastTimeAttacked = Time.time;
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = fireSpider.IsPlayerDetected().distance != 0 &&
             fireSpider.IsPlayerDetected().distance <= fireSpider.attackDistance &&
             (fireSpider.FacingDir == -1 && _player.transform.position.x <= fireSpider.transform.position.x ||
              fireSpider.FacingDir == 1 && _player.transform.position.x >= fireSpider.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - fireSpider.transform.position.x) < fireSpider.attackDistance &&
                Mathf.Abs(_player.transform.position.y - fireSpider.transform.position.y) <=
                fireSpider.CapsuleCollider.bounds.size.y)
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
