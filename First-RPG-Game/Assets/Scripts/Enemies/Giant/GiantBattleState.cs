using MainCharacter;
using UnityEngine;

namespace Enemies.Giant
{
    public class GiantBattleState : EnemyState
    {
        private Giant DarkBoss;
        private Transform _player;
        private int _moveDir;

        public GiantBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Giant giant) : base(enemyBase, stateMachine, animBoolName)
        {
            DarkBoss = giant;
        }
        public override void Enter()
        {
            base.Enter();

            // _player = GameObject.Find("Player").transform;
            AttachCurrentPlayerIfNotExists();

            //if player in attack range, block skeleton movement
            if (PlayerInAttackRange() && !CanAttack())
            {
                DarkBoss.SetZeroVelocity();
                StateMachine.ChangeState(DarkBoss.IdleState);
            }
        }
        public override void Update()
        {
            base.Update();

            if (DarkBoss.IsPlayerDetected())
            {
                StateTimer = DarkBoss.battleTime;

                if (DarkBoss.IsGroundDetected() && DarkBoss.IsPlayerDetected().distance <= DarkBoss.attackDistance &&
                    CanAttack())
                {
                    StateMachine.ChangeState(DarkBoss.AttackState);
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, DarkBoss.transform.position) > 7)
                {
                    StateMachine.ChangeState(DarkBoss.IdleState);
                }
            }

            _moveDir = _player.position.x > DarkBoss.transform.position.x ? 1 : -1;

            //if player in attack range, block skeleton movement
            if (PlayerInAttackRange())
            {
                DarkBoss.SetZeroVelocity();
                StateMachine.ChangeState(DarkBoss.IdleState);
                return;
            }

            DarkBoss.SetVelocity(DarkBoss.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }
        public override void Exit()
        {
            base.Exit();
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = DarkBoss.IsPlayerDetected().distance <= DarkBoss.attackDistance &&
                         (DarkBoss.FacingDir == -1 && _player.transform.position.x <= DarkBoss.transform.position.x ||
                          DarkBoss.FacingDir == 1 && _player.transform.position.x >= DarkBoss.transform.position.x);

            return result;
        }

        private void AttachCurrentPlayerIfNotExists()
        {
            if (!_player)
            {
                _player = PlayerManager.Instance.player.transform;
            }
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(DarkBoss.lastTimeAttacked, 0) || Time.time >= DarkBoss.lastTimeAttacked + DarkBoss.attackCooldown)
            {
                // _skeleton.lastTimeAttacked = Time.time;
                return true;
            }

            return false;
        }
    }
}

