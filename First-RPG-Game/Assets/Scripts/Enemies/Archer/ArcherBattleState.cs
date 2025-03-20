using MainCharacter;
using UnityEngine;

namespace Enemies.Archer
{
    public class ArcherBattleState : EnemyState
    {
        private EnemyArcher _archer;
        private Transform _player;
        private int _moveDir;

        public ArcherBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher enemy) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _archer = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            AttachCurrentPlayerIfNotExists();
            _archer.SetZeroVelocity();

            FaceToPlayer();
        }

        public override void Update()
        {
            base.Update();

            if (_archer.IsPlayerDetected() && _archer.IsPlayerDetected().distance != 0)
            {
                StateTimer = _archer.battleTime;

                if (_archer.IsPlayerDetected().distance < _archer.safeDistance)
                {
                    if (CanJump())
                    {
                        StateMachine.ChangeState(_archer.JumpState);
                        return;
                    }

                    if (
                        _archer.IsPlayerDetected().distance < _archer.meleeAttackDistance)
                    {
                        if (CanMeleeAttack())
                        {
                            StateMachine.ChangeState(_archer.MeleeAttackState);
                            return;
                        }
                    }
                    else
                    {
                        StateMachine.ChangeState(_archer.RunState);
                        return;
                    }
                }

                if (_archer.IsGroundDetected() &&
                    _archer.IsPlayerDetected().distance <= ((Enemy)_archer).attackDistance &&
                    CanAttack())
                {
                    StateMachine.ChangeState(_archer.AttackState);
                    return;
                }
            }


            if (_player.transform.position.x > _archer.transform.position.x && _archer.FacingDir == -1)
                _archer.Flip();
            else if (_player.transform.position.x < _archer.transform.position.x && _archer.FacingDir == 1)
                _archer.Flip();
        }

        private bool CanMeleeAttack()
        {
            return Time.time >= _archer.lastTimeAttacked + _archer.attackCooldown && _archer.IsPlayerDetected().distance != 0 &&
                   _archer.IsPlayerDetected().distance < 2f;
        }

        public override void Exit()
        {
            _archer.SetZeroVelocity();
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(_archer.lastTimeAttacked, 0) ||
                Time.time >= _archer.lastTimeAttacked + _archer.attackCooldown)
            {
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = _archer.IsPlayerDetected().distance != 0 &&
                         _archer.IsPlayerDetected().distance <= ((Enemy)_archer).attackDistance &&
                         (_archer.FacingDir == -1 && _player.transform.position.x <= _archer.transform.position.x ||
                          _archer.FacingDir == 1 && _player.transform.position.x >= _archer.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - _archer.transform.position.x) < ((Enemy)_archer).attackDistance &&
                Mathf.Abs(_player.transform.position.y - _archer.transform.position.y) <=
                _archer.CapsuleCollider.bounds.size.y)
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

        private bool CanJump()
        {
            if (_archer.GroundBehindCheck() && Time.time >= _archer.lastTimeJumped + _archer.jumpCooldown)
            {
                return true;
            }

            return false;
        }

        private void FaceToPlayer()
        {
            if (_player.transform.position.x > _archer.transform.position.x && _archer.FacingDir == -1)
                _archer.Flip();
            else if (_player.transform.position.x < _archer.transform.position.x && _archer.FacingDir == 1)
                _archer.Flip();
        }
    }
}