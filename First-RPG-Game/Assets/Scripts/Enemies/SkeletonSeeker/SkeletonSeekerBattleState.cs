using MainCharacter;
using UnityEngine;

namespace Enemies.SkeletonSeeker
{
    public class SkeletonSeekerBattleState : EnemyState
    {
        private SkeletonSeeker _skeletonSeeker;
        private Transform _player;
        private int _moveDir;
        public SkeletonSeekerBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, SkeletonSeeker skeletonSeeker) : base(enemyBase, stateMachine, animBoolName)
        {
            _skeletonSeeker = skeletonSeeker;
        }
        public override void Enter()
        {
            base.Enter();
            AttachCurrentPlayerIfNotExists();
        }

        public override void Update()
        {
            base.Update();

            if (_skeletonSeeker.IsPlayerDetected())
            {
                StateTimer = _skeletonSeeker.battleTime;

                if (_skeletonSeeker.IsGroundDetected() &&
                    _skeletonSeeker.IsPlayerDetected().distance <= _skeletonSeeker.attackDistance &&
                    CanAttack())
                {
                    StateMachine.ChangeState(_skeletonSeeker.AttackState);
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, _skeletonSeeker.transform.position) > 7)
                {
                    StateMachine.ChangeState(_skeletonSeeker.IdleState);
                }
            }

            _moveDir = _player.position.x > _skeletonSeeker.transform.position.x ? 1 : -1;

            //if player in attack range, block skeleton movement
            if (PlayerInAttackRange())
            {
                _skeletonSeeker.SetZeroVelocity();
                //StateMachine.ChangeState(_skeletonSeeker.IdleState);
                return;
            }

            _skeletonSeeker.SetVelocity(_skeletonSeeker.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(_skeletonSeeker.lastTimeAttacked, 0) ||
                Time.time >= _skeletonSeeker.lastTimeAttacked + _skeletonSeeker.attackCooldown)
            {
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = _skeletonSeeker.IsPlayerDetected().distance != 0 &&
                         _skeletonSeeker.IsPlayerDetected().distance <= _skeletonSeeker.attackDistance &&
                         (_skeletonSeeker.FacingDir == -1 && _player.transform.position.x <= _skeletonSeeker.transform.position.x ||
                          _skeletonSeeker.FacingDir == 1 && _player.transform.position.x >= _skeletonSeeker.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - _skeletonSeeker.transform.position.x) < _skeletonSeeker.attackDistance &&
                Mathf.Abs(_player.transform.position.y - _skeletonSeeker.transform.position.y) <=
                _skeletonSeeker.CapsuleCollider.bounds.size.y)
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
