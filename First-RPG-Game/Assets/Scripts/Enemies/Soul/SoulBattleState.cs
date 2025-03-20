using Enemies.Archer;
using MainCharacter;
using UnityEngine;

namespace Enemies.Soul
{
    public class SoulBattleState : EnemyState
    {
        private EnemySoul _soul;
        private Transform _player;
        private int _moveDir;

        private float _defaultSpeed;

        public SoulBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySoul enemy) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _soul = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            _defaultSpeed = _soul.moveSpeed;

            _soul.moveSpeed = _soul.battleStateMoveSpeed;

            AttachCurrentPlayerIfNotExists();
        }

        public override void Update()
        {
            base.Update();

            if (_soul.IsPlayerDetected())
            {
                StateTimer = _soul.battleTime;

                if (_soul.IsPlayerDetected().distance < _soul.explodeDistance)
                {
                    //TODO Should use _soul.Stats.KillEntity() to trigger explosion + drop items
                    StateMachine.ChangeState(_soul.DeadState);
                }

                else if (_soul.IsPlayerDetected().distance <= _soul.attackDistance &&
                         CanAttack())
                {
                    StateMachine.ChangeState(_soul.AttackState);
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, _soul.transform.position) > 10)
                {
                    StateMachine.ChangeState(_soul.IdleState);
                }
            }

            _moveDir = _player.position.x > _soul.transform.position.x ? 1 : -1;

            _soul.SetVelocity(_soul.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();

            _soul.moveSpeed = _defaultSpeed;
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(_soul.lastTimeAttacked, 0) ||
                Time.time >= _soul.lastTimeAttacked + _soul.attackCooldown)
            {
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = _soul.IsPlayerDetected().distance != 0 &&
                         _soul.IsPlayerDetected().distance <= _soul.attackDistance &&
                         (_soul.FacingDir == -1 && _player.transform.position.x <= _soul.transform.position.x ||
                          _soul.FacingDir == 1 && _player.transform.position.x >= _soul.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - _soul.transform.position.x) < _soul.attackDistance &&
                Mathf.Abs(_player.transform.position.y - _soul.transform.position.y) <=
                _soul.CapsuleCollider.bounds.size.y)
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