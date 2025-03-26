using MainCharacter;
using UnityEngine;

namespace Enemies.FireMage
{
    public class FireMageBattleState : EnemyState
    {
        private EnemyFireMage fireMage;
        private Transform _player;
        private int _moveDir;

        public FireMageBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMage _fireMage) :
            base(enemyBase, stateMachine, animBoolName)
        {
            fireMage = _fireMage;
        }

        public override void Enter()
        {
            //Debug.Log("Im in battle state");
            base.Enter();

            // _player = GameObject.Find("Player").transform;
            AttachCurrentPlayerIfNotExists();

            //if player in attack range, block fireMage movement
            if (PlayerInAttackRange() && !CanAttack())
            {
                fireMage.SetZeroVelocity();
                StateMachine.ChangeState(fireMage.IdleState);
            }
        }

        public override void Update()
        {
            base.Update();

            if (fireMage.IsPlayerDetected())
            {
                StateTimer = fireMage.battleTime;

                if (fireMage.IsGroundDetected() && fireMage.IsPlayerDetected().distance <= fireMage.attackDistance + 25 && CanAttack())
                {
                    StateMachine.ChangeState(fireMage.AttackState);
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, fireMage.transform.position) > 10)
                {
                    //Debug.Log("Attack2");
                    StateMachine.ChangeState(fireMage.IdleState);
                }
            }

            _moveDir = _player.position.x > fireMage.transform.position.x ? 1 : -1;
            //Debug.Log(_moveDir);

            //if player in attack range, block fireMage movement
            if (PlayerInAttackRange())
            {
                fireMage.SetZeroVelocity();
                StateMachine.ChangeState(fireMage.IdleState);
                //return;
            }

            if (fireMage.IsWallDetected())
            {
                fireMage.Flip();
                fireMage.SetVelocity(fireMage.moveSpeed * _moveDir, Rb.linearVelocity.y);
            }
            else
                fireMage.SetVelocity(fireMage.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(fireMage.lastTimeAttacked, 0) || Time.time >= fireMage.lastTimeAttacked + fireMage.attackCooldown)
            {
                // _fireMage.lastTimeAttacked = Time.time;
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = fireMage.IsPlayerDetected().distance != 0 &&
                         fireMage.IsPlayerDetected().distance <= fireMage.attackDistance &&
                         (fireMage.FacingDir == -1 && _player.transform.position.x <= fireMage.transform.position.x ||
                          fireMage.FacingDir == 1 && _player.transform.position.x >= fireMage.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - fireMage.transform.position.x) < fireMage.attackDistance &&
                Mathf.Abs(_player.transform.position.y - fireMage.transform.position.y) <=
                fireMage.CapsuleCollider.bounds.size.y)
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
