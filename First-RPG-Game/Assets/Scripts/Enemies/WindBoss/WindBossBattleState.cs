using MainCharacter;
using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossBattleState : EnemyState
    {
        private WindBoss _windBoss;
        private Transform _player;
        private int _moveDir;

        public WindBossBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }

        public override void Enter()
        {
            base.Enter();

            AttachCurrentPlayerIfNotExists();

            // FaceToPlayer();
        }

        public override void Update()
        {
            base.Update();

            if (_windBoss.IsPlayerDetected() && _windBoss.IsPlayerDetected().distance != 0)
            {
                StateTimer = _windBoss.battleTime;

                if (_windBoss.IsPlayerDetected().distance < _windBoss.triggerLeapDistance)
                {
                    if (CanLeap())
                    {
                        StateMachine.ChangeState(_windBoss.LeapState);
                        return;
                    }

                    if (
                        _windBoss.IsPlayerDetected().distance < _windBoss.meleeAttackDistance)
                    {
                        if (CanMeleeAttack())
                        {
                            StateMachine.ChangeState(_windBoss.MeleeAttackState);
                            return;
                        }
                    }
                    else
                    {
                        StateMachine.ChangeState(_windBoss.MoveState);
                        return;
                    }
                }

                if (_windBoss.IsGroundDetected() &&
                    _windBoss.IsPlayerDetected().distance <= _windBoss.attackDistance &&
                    CanAttack())
                {
                    StateMachine.ChangeState(_windBoss.MeleeAttackState);
                    return;
                }
            }


            if (_player.transform.position.x > _windBoss.transform.position.x && _windBoss.FacingDir == -1)
                _windBoss.Flip();
            else if (_player.transform.position.x < _windBoss.transform.position.x && _windBoss.FacingDir == 1)
                _windBoss.Flip();
        }

        private bool CanMeleeAttack()
        {
            return Time.time >= _windBoss.lastTimeAttacked + _windBoss.attackCooldown && _windBoss.IsPlayerDetected().distance != 0 &&
                   _windBoss.IsPlayerDetected().distance < 2f;
        }

        public override void Exit()
        {
            _windBoss.SetZeroVelocity();
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(_windBoss.lastTimeAttacked, 0) ||
                Time.time >= _windBoss.lastTimeAttacked + _windBoss.attackCooldown)
            {
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = _windBoss.IsPlayerDetected().distance != 0 &&
                         _windBoss.IsPlayerDetected().distance <= _windBoss.attackDistance &&
                         (_windBoss.FacingDir == -1 && _player.transform.position.x <= _windBoss.transform.position.x ||
                          _windBoss.FacingDir == 1 && _player.transform.position.x >= _windBoss.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - _windBoss.transform.position.x) < _windBoss.attackDistance &&
                Mathf.Abs(_player.transform.position.y - _windBoss.transform.position.y) <=
                _windBoss.CapsuleCollider.bounds.size.y)
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

        private bool CanLeap()
        {
            if (_windBoss.GroundBehindCheck() && Time.time >= _windBoss.lastTimeLeaped + _windBoss.leapCoolDown)
            {
                return true;
            }

            return false;
        }

        private void FaceToPlayer()
        {
            if (_player.transform.position.x > _windBoss.transform.position.x && _windBoss.FacingDir == -1)
                _windBoss.Flip();
            else if (_player.transform.position.x < _windBoss.transform.position.x && _windBoss.FacingDir == 1)
                _windBoss.Flip();
        }
    }
}