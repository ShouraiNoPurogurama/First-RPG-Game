using MainCharacter;
using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossLeapState : EnemyState
    {
        private WindBoss _windBoss;
        private float _defaultGravity;
        private float _flyTime;
        private float _stopTime;
        private bool _isFlipped;

        public WindBossLeapState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log("Leap State Entered");

            _windBoss.lastTimeLeaped = Time.time;

            _defaultGravity = Rb.gravityScale;

            _flyTime = .25f;

            _stopTime = 1f;
            
            Rb.gravityScale = 0;
        }

        public override void Update()
        {
            base.Update();

            _flyTime -= Time.deltaTime;

            if (_flyTime > 0)
            {
                Rb.linearVelocity = new Vector2(0, 35);
            }

            if (_flyTime < 0)
            {
                Rb.linearVelocity = new Vector2(0, 0);
                _stopTime -= Time.deltaTime;

                if (_stopTime <= 0)
                    StateMachine.ChangeState(_windBoss.LeapAttackState);
            }
        }

        public override void Exit()
        {
            Rb.gravityScale = _defaultGravity;
            base.Exit();
        }
        
        
    }
}