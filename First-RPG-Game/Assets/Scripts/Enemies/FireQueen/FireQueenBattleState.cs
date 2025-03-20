using MainCharacter;
using UnityEngine;

namespace Enemies.FireQueen
{
    public class FireQueenBattleState : EnemyState
    {
        private EnemyFireQueen fireQueen;
        private Transform _player;
        private int _moveDir;

        public FireQueenBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireQueen _fireQueen) :
            base(enemyBase, stateMachine, animBoolName)
        {
            fireQueen = _fireQueen;
        }

        public override void Enter()
        {
            //Debug.Log("Im in battle state");
            base.Enter();

            // _player = GameObject.Find("Player").transform;
            AttachCurrentPlayerIfNotExists();

            //if player in attack range, block FireQueen movement
            if (PlayerInAttackRange() && !CanAttack())
            {
                fireQueen.SetZeroVelocity();
                StateMachine.ChangeState(fireQueen.IdleState);
            }
        }

        public override void Update()
        {
            base.Update();

            if (fireQueen.IsPlayerDetected())
            {
                StateTimer = fireQueen.battleTime;

                if (fireQueen.IsGroundDetected() && fireQueen.IsPlayerDetected().distance <= fireQueen.attackDistance && CanAttack())
                {
                    StateMachine.ChangeState(fireQueen.AttackState);
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, fireQueen.transform.position) > 10)
                {
                    //Debug.Log("Attack2");
                    StateMachine.ChangeState(fireQueen.IdleState);
                }
            }

            _moveDir = _player.position.x > fireQueen.transform.position.x ? 1 : -1;
            //Debug.Log(_moveDir);

            //if player in attack range, block FireQueen movement
            if (PlayerInAttackRange())
            {
                fireQueen.SetZeroVelocity();
                StateMachine.ChangeState(fireQueen.IdleState);
                return;
            }

            fireQueen.SetVelocity(fireQueen.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(fireQueen.lastTimeAttacked, 0) || Time.time >= fireQueen.lastTimeAttacked + fireQueen.attackCooldown)
            {
                // _fireQueen.lastTimeAttacked = Time.time;
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = fireQueen.IsPlayerDetected().distance <= fireQueen.attackDistance &&
                   (fireQueen.FacingDir == -1 && _player.transform.position.x <= fireQueen.transform.position.x ||
                    fireQueen.FacingDir == 1 && _player.transform.position.x >= fireQueen.transform.position.x);

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
