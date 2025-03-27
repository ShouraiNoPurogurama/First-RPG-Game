using MainCharacter;
using UnityEngine;

namespace Enemies.FireWorm
{
    public class FireWormBattleState : EnemyState
    {
        private EnemyFireWorm fireWorm;
        private Transform _player;
        private int _moveDir;

        public FireWormBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireWorm _fireWorm) :
            base(enemyBase, stateMachine, animBoolName)
        {
            fireWorm = _fireWorm;
        }

        public override void Enter()
        {
            //Debug.Log("Im in battle state");
            base.Enter();

            // _player = GameObject.Find("Player").transform;
            AttachCurrentPlayerIfNotExists();

            //if player in attack range, block fireWorm movement
            if (PlayerInAttackRange() && !CanAttack())
            {
                fireWorm.SetZeroVelocity();
                StateMachine.ChangeState(fireWorm.IdleState);
            }
        }

        public override void Update()
        {
            //Debug.Log("Attack bbbbbbbbbbbbbb");
            base.Update();

            if (fireWorm.IsPlayerDetected())
            {
                //Debug.Log("Attack aaaaa1");
                StateTimer = fireWorm.battleTime;

                if (fireWorm.IsGroundDetected() && fireWorm.IsPlayerDetected().distance <= fireWorm.attackDistance && CanAttack())
                {
                    //Debug.Log("Attack player");
                    StateMachine.ChangeState(fireWorm.AttackState);
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, fireWorm.transform.position) > 10)
                {
                    //Debug.Log("Attack2");
                    StateMachine.ChangeState(fireWorm.IdleState);
                }
            }


            _moveDir = _player.position.x > fireWorm.transform.position.x ? 1 : -1;
            //Debug.Log(_moveDir);

            //if player in attack range, block fireWorm movement
            if (PlayerInAttackRange())
            {
                fireWorm.SetZeroVelocity();
                StateMachine.ChangeState(fireWorm.IdleState);
                //return;
            }

            fireWorm.SetVelocity(fireWorm.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(fireWorm.lastTimeAttacked, 0) || Time.time >= fireWorm.lastTimeAttacked + fireWorm.attackCooldown)
            {
                // _fireWorm.lastTimeAttacked = Time.time;
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = fireWorm.IsPlayerDetected().distance != 0 &&
             fireWorm.IsPlayerDetected().distance <= fireWorm.attackDistance &&
             (fireWorm.FacingDir == -1 && _player.transform.position.x <= fireWorm.transform.position.x ||
              fireWorm.FacingDir == 1 && _player.transform.position.x >= fireWorm.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - fireWorm.transform.position.x) < fireWorm.attackDistance &&
                Mathf.Abs(_player.transform.position.y - fireWorm.transform.position.y) <=
                fireWorm.CapsuleCollider.bounds.size.y)
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
