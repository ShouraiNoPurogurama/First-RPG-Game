using Enemies.Swordman;
using MainCharacter;
using UnityEngine;

namespace Enemies.Swordman
{
    public class EnemySwordmanBattleState : EnemyState
    {
        private Transform _player;
        private EnemySwordman enemy;
        private int _moveDir;
        public EnemySwordmanBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySwordman _enemy) : base(enemyBase, stateMachine, animBoolName )
        { 
            this.enemy = _enemy;
        }

        public override void Enter()
        {
            base.Enter();
            AttachCurrentPlayerIfNotExists();
        }

        public override void Update()
        {
            base.Update();

            if (enemy.IsPlayerDetected())
            {
                StateTimer = enemy.battleTime;

                if (enemy.IsGroundDetected() &&
                    enemy.IsPlayerDetected().distance <= enemy.attackDistance &&
                    CanAttack())
                {
                    Debug.Log(enemy.IsPlayerDetected().collider.gameObject.name);
                    StateMachine.ChangeState(enemy.AttackState);
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, enemy.transform.position) > 7)
                {
                    StateMachine.ChangeState(enemy.IdleState);
                }
            }

            _moveDir = _player.position.x > enemy.transform.position.x ? 1 : -1;

            //if player in attack range, block skeleton movement
            if (PlayerInAttackRange())
            {
                enemy.SetZeroVelocity();
                //StateMachine.ChangeState(_skeleton.IdleState);
                return;
            }

            enemy.SetVelocity(enemy.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(enemy.lastTimeAttacked, 0) ||
                Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
            {
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = enemy.IsPlayerDetected().distance != 0 &&
                         enemy.IsPlayerDetected().distance <= enemy.attackDistance &&
                         (enemy.FacingDir == -1 && _player.transform.position.x <= enemy.transform.position.x ||
                          enemy.FacingDir == 1 && _player.transform.position.x >= enemy.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - enemy.transform.position.x) < enemy.attackDistance &&
                Mathf.Abs(_player.transform.position.y - enemy.transform.position.y) <=
                enemy.CapsuleCollider.bounds.size.y)
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
