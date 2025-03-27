using MainCharacter;
using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossSpinAttackState : EnemyState
    {
        private WindBoss _windBoss;
        private float _hitTimer;
        private float _hitCooldown = 0.3f; // Damage every 0.3s
        private float _baseSpinSpeed = 5f; // Base spin speed
        private float _currentSpinSpeed;
        private float _acceleration = 1.2f; // Increases speed over time
        private float _trackingStrength = 0.1f; // Controls how much the boss adjusts direction
        private bool _isDesperate;
        
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

            _currentSpinSpeed = _baseSpinSpeed;
            _isDesperate = _windBoss.Stats.currentHp <= _windBoss.Stats.maxHp.ModifiedValue * 0.15f;

            //Increase damage if boss is desperate
            if (_isDesperate)
            {
                _windBoss.spinDamageScale *= 1.5f;
                _currentSpinSpeed *= 1.3f;
                _acceleration *= 1.5f;
                Debug.Log("WindBoss Spin Attack is stronger in Desperation Mode!");
            }
        }

        public override void Update()
        {
            base.Update();

            _hitTimer -= Time.deltaTime;

            _currentSpinSpeed += _acceleration * Time.deltaTime;

            AdjustDirectionTowardsPlayer();

            _windBoss.Rb.linearVelocity = new Vector2(_windBoss.FacingDir * _currentSpinSpeed, _windBoss.Rb.linearVelocity.y);

            if (_hitTimer <= 0)
            {
                _hitTimer = _hitCooldown;
                DamagePlayer();
            }

            if (StateTimer <= 0)
            {
                _windBoss.lastTimeSpin = Time.time;
                SlowDownAndExit();
            }
        }

        private void AdjustDirectionTowardsPlayer()
        {
            var player = PlayerManager.Instance.player;
            if (player is null) return;

            float playerX = player.transform.position.x;
            float bossX = _windBoss.transform.position.x;

            if ((playerX > bossX && _windBoss.FacingDir == -1) ||
                (playerX < bossX && _windBoss.FacingDir == 1))
            {
                _windBoss.Flip();
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

        private void SlowDownAndExit()
        {
            _windBoss.StartCoroutine(SlowDownRoutine());
        }

        private System.Collections.IEnumerator SlowDownRoutine()
        {
            var slowdownTime = 1f;
            var initialSpeed = _currentSpinSpeed;
            var elapsedTime = 0f;

            while (elapsedTime < slowdownTime)
            {
                elapsedTime += Time.deltaTime;
                _windBoss.Rb.linearVelocity = new Vector2(_windBoss.FacingDir * Mathf.Lerp(initialSpeed, 0, elapsedTime / slowdownTime), _windBoss.Rb.linearVelocity.y);
                yield return null;
            }

            _windBoss.SetZeroVelocity();
            StateMachine.ChangeState(_windBoss.BattleState);
        }

        public override void Exit()
        {
            _windBoss.SetZeroVelocity();
            base.Exit();
        }
    }
}
