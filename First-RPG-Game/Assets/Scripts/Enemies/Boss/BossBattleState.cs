using MainCharacter;
using UnityEngine;

namespace Enemies.Boss
{
    public class BossBattleState : EnemyState
    {
        private EnemyBoss boss;
        private Transform _player;
        private int _moveDir;

        public BossBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyBoss _boss) :
            base(enemyBase, stateMachine, animBoolName)
        {
            boss = _boss;
        }

        public override void Enter()
        {
            //Debug.Log("Im in battle state");
            base.Enter();

            // _player = GameObject.Find("Player").transform;
            AttachCurrentPlayerIfNotExists();

            //if player in attack range, block boss movement
            if (PlayerInAttackRange() && !CanAttack())
            {
                boss.SetZeroVelocity();
                StateMachine.ChangeState(boss.IdleState);
            }
        }

        public override void Update()
        {
            base.Update();

            if (boss.IsPlayerDetected())
            {
                StateTimer = boss.battleTime;

                if (boss.IsGroundDetected() && boss.IsPlayerDetected().distance <= boss.attackDistance && CanAttack())
                {
                    //Debug.Log("Start Attack");
                    //StateMachine.ChangeState(boss.AttackState);
                    boss.attackManager.HandleAttackLogic();
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, boss.transform.position) > 10)
                {
                    //Debug.Log("Attack2");
                    StateMachine.ChangeState(boss.IdleState);
                }
            }

            _moveDir = _player.position.x > boss.transform.position.x ? 1 : -1;
            //Debug.Log(_moveDir);

            //if player in attack range, block boss movement
            if (PlayerInAttackRange())
            {
                Debug.Log("Loi ne");
                boss.SetZeroVelocity();
                //boss.attackManager.HandleAttackLogic();
                StateMachine.ChangeState(boss.IdleState);
                //return;
            }

            if (boss.IsWallDetected())
            {
                boss.Flip();
                boss.SetVelocity(boss.moveSpeed * _moveDir, Rb.linearVelocity.y);
            }
            else
                boss.SetVelocity(boss.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(boss.lastTimeAttacked, 0) || Time.time >= boss.lastTimeAttacked)
            {
                //Debug.Log(Time.time >= boss.lastTimeAttacked + boss.attackCooldown);
                // _boss.lastTimeAttacked = Time.time;
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = boss.IsPlayerDetected().distance != 0 &&
                     boss.IsPlayerDetected().distance <= boss.attackDistance &&
                     (boss.FacingDir == -1 && _player.transform.position.x <= boss.transform.position.x ||
                      boss.FacingDir == 1 && _player.transform.position.x >= boss.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - boss.transform.position.x) < boss.attackDistance &&
                Mathf.Abs(_player.transform.position.y - boss.transform.position.y) <=
                boss.CapsuleCollider.bounds.size.y)
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
