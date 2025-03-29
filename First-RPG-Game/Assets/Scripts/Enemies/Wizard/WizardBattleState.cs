using MainCharacter;
using UnityEngine;

namespace Enemies.Wizard
{
    public class WizardBattleState : EnemyState
    {
        private EnemyWizard _wizard;
        private Transform _player;
        private int _moveDir;

        public WizardBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyWizard wizard) :
            base(enemyBase, stateMachine, animBoolName)
        {
            _wizard = wizard;
        }

        public override void Enter()
        {
            base.Enter();
            AttachCurrentPlayerIfNotExists();
        }

        public override void Update()
        {
            base.Update();

            if (_wizard.IsPlayerDetected())
            {
                StateTimer = _wizard.battleTime;

                if (_wizard.IsGroundDetected() &&
                    _wizard.IsPlayerDetected().distance <= _wizard.attackDistance &&
                    CanAttack())
                {
                    Debug.Log(_wizard.IsPlayerDetected().collider.gameObject.name);
                    StateMachine.ChangeState(_wizard.AttackState);
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, _wizard.transform.position) > 7)
                {
                    StateMachine.ChangeState(_wizard.IdleState);
                }
            }

            _moveDir = _player.position.x > _wizard.transform.position.x ? 1 : -1;

            if (PlayerInAttackRange())
            {
                _wizard.SetZeroVelocity();
                return;
            }

            _wizard.SetVelocity(_wizard.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(_wizard.lastTimeAttacked, 0) ||
                Time.time >= _wizard.lastTimeAttacked + _wizard.attackCooldown)
            {
                return true;
            }
            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = _wizard.IsPlayerDetected().distance != 0 &&
                         _wizard.IsPlayerDetected().distance <= _wizard.attackDistance &&
                         (_wizard.FacingDir == -1 && _player.transform.position.x <= _wizard.transform.position.x ||
                          _wizard.FacingDir == 1 && _player.transform.position.x >= _wizard.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - _wizard.transform.position.x) < _wizard.attackDistance &&
                Mathf.Abs(_player.transform.position.y - _wizard.transform.position.y) <=
                _wizard.CapsuleCollider.bounds.size.y)
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