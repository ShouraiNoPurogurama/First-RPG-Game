using MainCharacter;
using UnityEngine;

namespace Enemies.Frogger
{
    public class FroggerBattleState : EnemyState
    {
        private EnemyFrogger _frogger;
        private Transform _player;
        private int _moveDir;

        public FroggerBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFrogger enemy) :
            base(
                enemyBase, stateMachine, animBoolName)
        {
            _frogger = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            AttachCurrentPlayerIfNotExists();
            _frogger.SetZeroVelocity();
        }

        public override void Update()
        {
            base.Update();

            if (_frogger.IsPlayerDetected())
            {
                StateTimer = _frogger.battleTime;

                if (_frogger.Stats.currentHp <= 0.4f * _frogger.Stats.maxHp.ModifiedValue && CanRegen())
                {
                    StateMachine.ChangeState(_frogger.JumpState);
                    return;
                }
                
                if (_frogger.IsPlayerDetected().distance < _frogger.spitDistance)
                {
                    if (CanSpitAttack())
                    {
                        StateMachine.ChangeState(_frogger.SpitAttackState);
                        return;
                    }
                }

                if (_frogger.IsPlayerDetected().distance < _frogger.attackDistance && CanAttack())
                {
                    StateMachine.ChangeState(_frogger.TongueAttackState);
                    return;
                }
            }

            _moveDir = _player.position.x > _frogger.transform.position.x ? 1 : -1;

            if (PlayerInAttackRange())
            {
                _frogger.SetZeroVelocity();
                StateMachine.ChangeState(_frogger.IdleState);
                return;
            }

            _frogger.SetVelocity(_frogger.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        private bool CanSpitAttack()
        {
            return Time.time >= _frogger.lastTimeSpit + _frogger.spitCooldown;
        }

        public override void Exit()
        {
            _frogger.SetZeroVelocity();
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(_frogger.lastTimeAttacked, 0) ||
                Time.time >= _frogger.lastTimeAttacked + _frogger.attackCooldown)
            {
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = _frogger.IsPlayerDetected().distance != 0 &&
                         _frogger.IsPlayerDetected().distance <= _frogger.attackDistance &&
                         (_frogger.FacingDir == -1 && _player.transform.position.x <= _frogger.transform.position.x ||
                          _frogger.FacingDir == 1 && _player.transform.position.x >= _frogger.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - _frogger.transform.position.x) < _frogger.attackDistance &&
                Mathf.Abs(_player.transform.position.y - _frogger.transform.position.y) <=
                _frogger.CapsuleCollider.bounds.size.y)
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

        private bool CanRegen()
        {
            if (Time.time >= _frogger.lastTimeRegen + _frogger.regenCooldown)
            {
                return true;
            }

            return false;
        }
    }
}