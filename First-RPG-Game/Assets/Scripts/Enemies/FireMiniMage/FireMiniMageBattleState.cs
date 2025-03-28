using MainCharacter;
using UnityEngine;

namespace Enemies.FireMiniMage
{
    public class FireMiniMageBattleState : EnemyState
    {
        private EnemyFireMiniMage FireMiniMage;
        private Transform _player;
        private int _moveDir;

        public FireMiniMageBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMiniMage _FireMiniMage) :
            base(enemyBase, stateMachine, animBoolName)
        {
            FireMiniMage = _FireMiniMage;
        }

        public override void Enter()
        {
            //Debug.Log("Im in battle state");
            base.Enter();

            // _player = GameObject.Find("Player").transform;
            AttachCurrentPlayerIfNotExists();

            //if player in attack range, block FireMiniMage movement
            if (PlayerInAttackRange() && !CanAttack())
            {
                FireMiniMage.SetZeroVelocity();
                StateMachine.ChangeState(FireMiniMage.IdleState);
            }
        }

        public override void Update()
        {
            base.Update();

            if (FireMiniMage.IsPlayerDetected() && FireMiniMage.IsPlayerDetected().distance != 0)
            {
                StateTimer = FireMiniMage.battleTime;

                if (FireMiniMage.IsGroundDetected() && FireMiniMage.IsPlayerDetected().distance <= FireMiniMage.attackDistance && CanAttack())
                {
                    StateMachine.ChangeState(FireMiniMage.AttackState);
                    return;
                }

                if (FireMiniMage.IsGroundDetected() && FireMiniMage.IsPlayerDetected().distance <= FireMiniMage.attackDistance)
                {
                       if (CanThrowBallAttack())
                       {
                           StateMachine.ChangeState(FireMiniMage.ThrowAttackState);
                           return;
                       }
                }
                
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, FireMiniMage.transform.position) > 10)
                {
                    //Debug.Log("Attack2");
                    StateMachine.ChangeState(FireMiniMage.IdleState);
                }
            }

            _moveDir = _player.position.x > FireMiniMage.transform.position.x ? 1 : -1;
            //Debug.Log(_moveDir);

            //if player in attack range, block FireMiniMage movement
            if (PlayerInAttackRange())
            {
                FireMiniMage.SetZeroVelocity();
                StateMachine.ChangeState(FireMiniMage.IdleState);
                //return;
            }

            if (FireMiniMage.IsWallDetected())
            {
                FireMiniMage.Flip();
                FireMiniMage.SetVelocity(FireMiniMage.moveSpeed * _moveDir, Rb.linearVelocity.y);
            }
            else
                FireMiniMage.SetVelocity(FireMiniMage.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }
        private bool CanThrowBallAttack()
        {
            return Time.time >= FireMiniMage.lastTimeAttacked + FireMiniMage.attackCooldown && FireMiniMage.IsPlayerDetected().distance != 0 &&
                   FireMiniMage.IsPlayerDetected().distance <= FireMiniMage.throwballDistance;
        }
        public override void Exit()
        {
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(FireMiniMage.lastTimeAttacked, 0) || 
                Time.time >= FireMiniMage.lastTimeAttacked + FireMiniMage.attackCooldown)
            {
                // _FireMiniMage.lastTimeAttacked = Time.time;
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = FireMiniMage.IsPlayerDetected().distance != 0 &&
                         FireMiniMage.IsPlayerDetected().distance <= FireMiniMage.attackDistance &&
                         (FireMiniMage.FacingDir == -1 && _player.transform.position.x <= FireMiniMage.transform.position.x ||
                          FireMiniMage.FacingDir == 1 && _player.transform.position.x >= FireMiniMage.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - FireMiniMage.transform.position.x) < FireMiniMage.attackDistance &&
                Mathf.Abs(_player.transform.position.y - FireMiniMage.transform.position.y) <=
                FireMiniMage.CapsuleCollider.bounds.size.y)
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
