using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossLeapState : EnemyState
    {
        private WindBoss _windBoss;
        private float _defaultGravity;
        private float _flyTime = 1.25f;

        public WindBossLeapState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }

        public override void Enter()
        {
            base.Enter();
            
            _windBoss.lastTimeLeaped = Time.time;
            
            _defaultGravity = Rb.gravityScale;

            StateTimer = _flyTime;
            
            Rb.gravityScale = 0;
        }

        public override void Update()
        {
            base.Update();
            
            if (StateTimer > 0)
            {
                Rb.linearVelocity = new Vector2(0, 10);
            }

            if (StateTimer < 0)
            {
                Rb.linearVelocity = new Vector2(0, -.1f);

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