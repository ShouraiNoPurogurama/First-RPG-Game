using MainCharacter;
using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossTauntState : EnemyState
    {
        private WindBoss _windBoss;
        private float _desperationDuration = 3f; // Taunt duration before power-up
        private float _lastTimeRegenInterval;
        private int _regenCount;
        private int _armorModifier;
        private float _originalMoveSpeed;
        private float _originalAttackCooldown;
        private float _originalSpinCooldown;

        public WindBossTauntState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) 
            : base(enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }

        public override void Enter()
        {
            base.Enter();
            _windBoss.SetZeroVelocity();
            
            _regenCount = 3;
            
            _originalMoveSpeed = _windBoss.defaultMoveSpeed;
            _originalAttackCooldown = _windBoss.attackCooldown;
            _originalSpinCooldown = _windBoss.spinCoolDown;

            StateTimer = _desperationDuration;

            int playerDamage = PlayerManager.Instance.player.Stats.damage.ModifiedValue;
            _armorModifier = Mathf.RoundToInt(0.7f * playerDamage); //Buff armor by 70% of player damage
            _windBoss.Stats.armor.AddModifier(_armorModifier);

            Debug.Log($"WindBoss gained armor: {_windBoss.Stats.armor}");
        }

        public override void Update()
        {
            base.Update();
            StateTimer -= Time.deltaTime;

            float regenInterval = _desperationDuration / 3;
            if (_regenCount > 0 && Time.time >= _lastTimeRegenInterval + regenInterval)
            {
                int regenAmount = Mathf.RoundToInt(_windBoss.Stats.maxHp.ModifiedValue * 0.2f);
                _windBoss.Stats.RecoverHPBy(regenAmount);
                _lastTimeRegenInterval = Time.time;
                _regenCount--;

                Debug.Log($"WindBoss healed for: {regenAmount} HP");
            }

            if (StateTimer <= 0 && ! _windBoss.enteredTaunt)
            {
                ActivateDesperationMode();
            }
        }

        private void ActivateDesperationMode()
        {
            _windBoss.enteredTaunt = true;

            //Increase speed & attack power
            _windBoss.defaultMoveSpeed *= 1.3f;
            _windBoss.attackCooldown *= 0.7f;
            _windBoss.spinCoolDown *= 0.7f;
            _windBoss.summonCoolDown *= 0.7f;
            _windBoss.Stats.damage.AddModifier(Mathf.RoundToInt(_windBoss.Stats.damage.ModifiedValue * .25f));

            Debug.Log("WindBoss entered Desperation Mode!");

            StateMachine.ChangeState(_windBoss.BattleState);
        }

        public override void Exit()
        {
            base.Exit();

            // Remove armor buff
            _windBoss.Stats.armor.RemoveModifier(_armorModifier);
            Debug.Log($"WindBoss lost armor: {_windBoss.Stats.armor}");

            // Restore original stats
            // _windBoss.defaultMoveSpeed = _originalMoveSpeed;
            // _windBoss.attackCooldown = _originalAttackCooldown;
            // _windBoss.spinCoolDown = _originalSpinCooldown;
        }
    }
}
