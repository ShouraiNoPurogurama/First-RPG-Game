using UnityEngine;

namespace Enemies.DarkBoss
{
    public class DarkBossSummonState : EnemyState
    {
        private DarkBoss _darkBoss;
        private Vector2 _targetPosition;
        private float defaultGravityScale;
        private float _moveSpeed = 5f;
        private float _lastSummonTime = 0f;
        private float _summonInterval = 5f; // Summon every 5 seconds

        public DarkBossSummonState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, DarkBoss darkBoss) : base(enemyBase, stateMachine, animBoolName)
        {
            _darkBoss = darkBoss;
            _targetPosition = new Vector2(0, 0);
        }

        public override void Enter()
        {
            base.Enter();
            _darkBoss.IsCallSummoned = false;
            defaultGravityScale = _darkBoss.Rb.gravityScale;
            _darkBoss.Rb.gravityScale = 0;
            StateTimer = 30f;
            MoveToCenter();
            _lastSummonTime = Time.time; // Initialize summon timer
        }

        public override void Update()
        {
            base.Update();
            _darkBoss.transform.position = Vector2.MoveTowards(_darkBoss.transform.position, _targetPosition, _moveSpeed * Time.deltaTime);


            // When reaching the target position, ensure summoning continues until the state changes
            if (Vector2.Distance(_darkBoss.transform.position, _targetPosition) < 0.1f)
            {
                if (Time.time - _lastSummonTime >= _summonInterval)
                {
                    _darkBoss.Summon();
                    _lastSummonTime = Time.time; 
                }
            }

            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(_darkBoss.IdleState);
            }
        }

        public override void Exit()
        {
            _darkBoss.Rb.gravityScale = defaultGravityScale;
            base.Exit();
        }

        private void MoveToCenter()
        {
            // Set the target position to the center of the arena
            _targetPosition = _darkBoss.arena.bounds.center;
        }
    }
}
