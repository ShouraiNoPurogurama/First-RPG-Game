using MainCharacter;
using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossSpinAttackState : EnemyState
    {
        private WindBoss _windBoss;
        private float _hitTimer;
        private float _hitCooldown = 0.3f; // Damage every 0.3s
        private float _spinSpeed = 5f; // Adjust as needed

        public WindBossSpinAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) 
            : base(enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }
        
        public override void Enter()
        {
            base.Enter();
            _hitTimer = _hitCooldown;
            StateTimer = _windBoss.spinDuration;
        }

        public override void Update()
        {
            base.Update();

            _windBoss.Rb.linearVelocity = new Vector2(_windBoss.FacingDir * _spinSpeed, _windBoss.Rb.linearVelocity.y);

            _hitTimer -= Time.deltaTime;
            if (_hitTimer <= 0)
            {
                _hitTimer = _hitCooldown;
                DamagePlayer();
            }

            if (StateTimer <= 0)
            {
                _windBoss.lastTimeSpin = Time.time;
                StateMachine.ChangeState(_windBoss.BattleState);
            }
        }

        private void DamagePlayer()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_windBoss.transform.position, 1.5f);
            
            foreach (var hit in colliders)
            {
                var player = hit.GetComponent<Player>();
                if (player is not null)
                {
                    _windBoss.Stats.DoMagicalDamage(player.Stats, _windBoss.spinDamageScale);
                }
            }
        }

        public override void Exit()
        {
            _windBoss.SetZeroVelocity();
            base.Exit();
        }
    }
}