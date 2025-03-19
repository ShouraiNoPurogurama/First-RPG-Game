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

            if (FireMiniMage.IsPlayerDetected())
            {
                StateTimer = FireMiniMage.battleTime;

                if (FireMiniMage.IsGroundDetected() && FireMiniMage.IsPlayerDetected().distance <= FireMiniMage.attackDistance && CanAttack())
                {
                    StateMachine.ChangeState(FireMiniMage.AttackState);
                    return;
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
                return;
            }

            FireMiniMage.SetVelocity(FireMiniMage.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(FireMiniMage.lastTimeAttacked, 0) || Time.time >= FireMiniMage.lastTimeAttacked + FireMiniMage.attackCooldown)
            {
                // _FireMiniMage.lastTimeAttacked = Time.time;
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = FireMiniMage.IsPlayerDetected().distance <= FireMiniMage.attackDistance &&
                   (FireMiniMage.FacingDir == -1 && _player.transform.position.x <= FireMiniMage.transform.position.x ||
                    FireMiniMage.FacingDir == 1 && _player.transform.position.x >= FireMiniMage.transform.position.x);

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
