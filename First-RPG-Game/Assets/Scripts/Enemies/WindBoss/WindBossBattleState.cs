using MainCharacter;
using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossBattleState : EnemyState
    {
        private WindBoss _windBoss;
        private Transform _player;
        private int _moveDir;
        private bool playedTauntFX;

        public WindBossBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) :
            base(
                enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }

        public override void Enter()
        {
            base.Enter();

            AttachCurrentPlayerIfNotExists();

            _windBoss.SetVelocity(_windBoss.defaultMoveSpeed * _windBoss.FacingDir, Rb.linearVelocity.y);
        }

        public override void Update()
        {
            base.Update();

            float playerDistance = _windBoss.IsPlayerDetected().distance;
            bool playerDetected = playerDistance != 0;
            bool isLowHp = _windBoss.Stats.currentHp <= _windBoss.Stats.maxHp.ModifiedValue * 0.5;
            bool isDesperate = _windBoss.Stats.currentHp <= _windBoss.Stats.maxHp.ModifiedValue * 0.25;

            if (isLowHp && !playedTauntFX)
            {
                playedTauntFX = true;
                _windBoss.FX.StartTauntFX();
                _windBoss.FX.IncreaseFallingLeavesFX();
            }

            if (isDesperate)
            {
                _windBoss.FX.IncreaseTauntFX();
                _windBoss.FX.IncreaseFallingLeavesFX();
            }

            if (playerDetected)
            {
                StateTimer = _windBoss.battleTime;

                if (isLowHp && playerDistance < _windBoss.triggerLeapDistance && CanLeap())
                {
                    StateMachine.ChangeState(_windBoss.LeapState);
                    return;
                }

                if (Mathf.Abs(playerDistance - _windBoss.attackDistance) <=10 && Mathf.Abs(playerDistance - _windBoss.attackDistance) >= 4 && CanDash())
                {
                    StateMachine.ChangeState(_windBoss.DashState);
                    return;
                }
                
                if (CanSummonMinions())
                {
                    StateMachine.ChangeState(_windBoss.SummonState);
                    return;
                }

                if (playerDistance < _windBoss.attackDistance)
                {
                    if (CanMeleeAttack())
                    {
                        StateMachine.ChangeState(_windBoss.MeleeAttackState);
                        return;
                    }

                    if (CanSpinAttack())
                    {
                        StateMachine.ChangeState(_windBoss.EnterSpinAttackState);
                        return;
                    }
                }
            }
            
            if (isDesperate && CanDesperationMode())
            {
                StateMachine.ChangeState(_windBoss.TauntState);
                return;
            }

            HandleFlipping();

            _windBoss.SetVelocity(_windBoss.moveSpeed * 1.1f * _windBoss.FacingDir, Rb.linearVelocity.y);
        }

        private void HandleFlipping()
        {
            if (_player.transform.position.x > _windBoss.transform.position.x && _windBoss.FacingDir == -1)
                _windBoss.Flip();
            else if (_player.transform.position.x < _windBoss.transform.position.x && _windBoss.FacingDir == 1)
                _windBoss.Flip();
        }

        private bool CanMeleeAttack()
        {
            return Time.time >= _windBoss.lastTimeAttacked + _windBoss.attackCooldown &&
                   _windBoss.IsPlayerDetected().distance != 0 &&
                   _windBoss.IsPlayerDetected().distance < 2f;
        }

        public override void Exit()
        {
            _windBoss.SetZeroVelocity();
            base.Exit();
        }

        public bool CanDash() => Time.time >= _windBoss.lastTimeDashed + _windBoss.dashCooldown;

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(_windBoss.lastTimeAttacked, 0) ||
                Time.time >= _windBoss.lastTimeAttacked + _windBoss.attackCooldown)
            {
                return true;
            }

            return false;
        }

        private bool CanDesperationMode() => !_windBoss.enteredTaunt;

        public bool CanSpinAttack()
        {
            AttachCurrentPlayerIfNotExists();

            if (Mathf.Approximately(_windBoss.lastTimeAttacked, 0) ||
                Time.time >= _windBoss.lastTimeSpin + _windBoss.spinCoolDown)
            {
                return true;
            }

            return false;
        }

        private bool CanSummonMinions()
        {
            return Time.time >= _windBoss.lastTimeSummon + _windBoss.summonCoolDown;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();

            var result = _windBoss.IsPlayerDetected().distance != 0 &&
                         _windBoss.IsPlayerDetected().distance <= _windBoss.attackDistance &&
                         (_windBoss.FacingDir == -1 && _player.transform.position.x <= _windBoss.transform.position.x ||
                          _windBoss.FacingDir == 1 && _player.transform.position.x >= _windBoss.transform.position.x);

            if (Mathf.Abs(_player.transform.position.x - _windBoss.transform.position.x) < _windBoss.attackDistance &&
                Mathf.Abs(_player.transform.position.y - _windBoss.transform.position.y) <=
                _windBoss.CapsuleCollider.bounds.size.y)
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

        private bool CanLeap()
        {
            if (_windBoss.GroundBehindCheck() && Time.time >= _windBoss.lastTimeLeaped + _windBoss.leapCoolDown)
            {
                return true;
            }

            return false;
        }
    }
}