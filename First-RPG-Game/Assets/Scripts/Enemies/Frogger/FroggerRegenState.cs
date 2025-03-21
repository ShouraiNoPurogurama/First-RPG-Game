using MainCharacter;
using UnityEngine;

namespace Enemies.Frogger
{
    public class FroggerRegenState : EnemyState
    {
        private EnemyFrogger _frogger;
        private float _lastTimeRegenInterval;
        private int _regenCount;
        private int _playerDamage;
        private int _playerMagicalDamage;
        private Vector3 _initialWallCheckPosition;

        public FroggerRegenState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFrogger enemy) :
            base(
                enemyBase, stateMachine, animBoolName)
        {
            _frogger = enemy;
        }

        public override void Enter()
        {
            base.Enter();
            _frogger.SetZeroVelocity();
            _frogger.lastTimeRegen = Time.time;
            StateTimer = _frogger.regenDuration;
            _regenCount = 3;

            _playerDamage = PlayerManager.Instance.player.Stats.damage.ModifiedValue;

            _initialWallCheckPosition = _frogger.wallCheck.position;

            _frogger.Stats.armor.AddModifier(Mathf.RoundToInt(0.5f * _playerDamage));
            Debug.Log("armor added" + _frogger.Stats.armor);
        }

        public override void Update()
        {
            base.Update();

            StateTimer -= Time.deltaTime;

            //time interval for each regeneration step
            float regenInterval = _frogger.regenDuration / 3;

            _frogger.SetVelocity(0, _frogger.Rb.linearVelocity.y);

            if (Time.time >= _lastTimeRegenInterval + regenInterval)
            {
                var regenAmount = Mathf.RoundToInt(_frogger.regenScaleByHp * _frogger.Stats.maxHp.ModifiedValue / 3);
                Debug.Log("Regen amount: " + regenAmount);
                _frogger.Stats.RecoverHPBy(regenAmount);
                _lastTimeRegenInterval = Time.time;
                _regenCount--;
            }

            var scaleFactor = 1 + (_frogger.sizeScaleAfterRegen * Time.deltaTime / _frogger.regenDuration);

            ScaleFroggerBy(scaleFactor);

            _frogger.wallCheck.position = _initialWallCheckPosition;
            
            if (StateTimer <= 0 && _regenCount <= 0)
            {
                StateMachine.ChangeState(_frogger.BattleState);
            }
        }

        private void ScaleFroggerBy(float scaleFactor)
        {
            _frogger.Animator.transform.localScale = new Vector3(
                _frogger.Animator.transform.localScale.x * scaleFactor,
                _frogger.Animator.transform.localScale.y * scaleFactor,
                _frogger.Animator.transform.localScale.z
            );

            _frogger.CapsuleCollider.size *= scaleFactor;
            _frogger.spitDistance *= scaleFactor;
            _frogger.attackCheckRadius *= scaleFactor;
            _frogger.counterImage.transform.localScale *= scaleFactor;
            _frogger.groundCheckDistance *= (0.1f * Time.deltaTime + scaleFactor);
        }

        public override void Exit()
        {
            _frogger.Stats.armor.RemoveModifier(Mathf.RoundToInt(0.5f * _playerDamage));
            Debug.Log("Armor removed" + _frogger.Stats.armor);
            base.Exit();
        }
    }
}