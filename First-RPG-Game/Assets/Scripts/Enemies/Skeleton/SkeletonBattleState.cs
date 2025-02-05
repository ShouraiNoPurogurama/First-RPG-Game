using UnityEngine;

namespace Enemies.Skeleton
{
    public class SkeletonBattleState : EnemyState
    {
        private EnemySkeleton _skeleton;
        private Transform _player;
        private int _moveDir;

        public SkeletonBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton skeleton) :
            base(enemyBase, stateMachine, animBoolName)
        {
            _skeleton = skeleton;
        }

        public override void Enter()
        {
            base.Enter();

            // _player = GameObject.Find("Player").transform;
            _player = PlayerManager.Instance.player.transform;
        }

        public override void Update()
        {
            base.Update();

            if (_skeleton.IsPlayerDetected())
            {
                StateTimer = _skeleton.battleTime;
                if (_skeleton.IsPlayerDetected().distance < _skeleton.attackDistance && CanAttack())
                {
                    StateMachine.ChangeState(_skeleton.AttackState);
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, _skeleton.transform.position) > 7)
                {
                    StateMachine.ChangeState(_skeleton.IdleState);
                }
            }

            _moveDir = _player.position.x > _skeleton.transform.position.x ? 1 : -1;
        
            _skeleton.SetVelocity(_skeleton.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }

        private bool CanAttack()
        {
            if (Time.time >= _skeleton.lastTimeAttacked + _skeleton.attackCooldown)
            {
                _skeleton.lastTimeAttacked = Time.time;
                return true;
            }

            return false;
        }
    }
}