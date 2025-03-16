using MainCharacter;
using UnityEngine;

namespace Enemies.Map_Water.Magic_Skeleton
{
    public class MagicSkeletonBattleState : EnemyState
    {
        private Enemy_Magic_Skeleton magicSkeleton;
        private Transform _player;
        private int _moveDir;

        public MagicSkeletonBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Magic_Skeleton enemy) : base(enemyBase, stateMachine, animBoolName)
        {
            magicSkeleton = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            // _player = GameObject.Find("Player").transform;
            AttachCurrentPlayerIfNotExists();
            Debug.Log("Magic Skeleton Battle State");
        }

        public override void Update()
        {
            base.Update();

            if (magicSkeleton.IsPlayerDetected())
            {
                StateTimer = magicSkeleton.battleTime;
                if (magicSkeleton.IsPlayerDetected().distance < magicSkeleton.safeDistance)
                {
                    if (CanJump())
                    {
                        StateMachine.ChangeState(magicSkeleton.JumpState);
                        return;
                    }
                }
                if (magicSkeleton.IsPlayerDetected().distance < magicSkeleton.attackDistance &&
                    CanAttack())
                {
                    Debug.Log("Magic Skeleton Attack State");
                    StateMachine.ChangeState(magicSkeleton.AttackState);
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, magicSkeleton.transform.position) > 7)
                {
                    Debug.Log("Magic Skeleton Move State");
                    StateMachine.ChangeState(magicSkeleton.IdleState);
                }
            }

            //_moveDir = _player.position.x > magicSkeleton.transform.position.x ? 1 : -1;
            if (_player.position.x > magicSkeleton.transform.position.x && magicSkeleton.FacingDir == -1)
                magicSkeleton.Flip();
            else if (_player.position.x < magicSkeleton.transform.position.x && magicSkeleton.FacingDir == 1)
                magicSkeleton.Flip();
            //magicSkeleton.SetVelocity(magicSkeleton.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(magicSkeleton.lastTimeAttacked, 0) || Time.time >= magicSkeleton.lastTimeAttacked + magicSkeleton.attackCooldown)
            {
                magicSkeleton.lastTimeAttacked = Time.time;
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();
            var hit = magicSkeleton.IsPlayerDetected();
            if (hit.collider == null)
                return false;

            bool inRange = hit.distance <= magicSkeleton.attackDistance;
            bool correctDirection = (magicSkeleton.FacingDir == -1 && _player.position.x <= magicSkeleton.transform.position.x) ||
                                    (magicSkeleton.FacingDir == 1 && _player.position.x >= magicSkeleton.transform.position.x);
            return inRange && correctDirection;
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
            if (magicSkeleton.GroundBehind() == false || magicSkeleton.WallBehind())
                return false;

            if (Time.time >= magicSkeleton.lastTimeJumped + magicSkeleton.JumpCooldown)
            {
                magicSkeleton.lastTimeJumped = Time.time;
                return true;
            }
            return false;
        }
    }
}
