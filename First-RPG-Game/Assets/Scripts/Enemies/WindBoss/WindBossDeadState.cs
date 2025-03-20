using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossDeadState : EnemyState
    {
        private WindBoss _windBoss;

        public WindBossDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }

        public override void Enter()
        {
            base.Enter();

            _windBoss.Animator.SetBool(_windBoss.LastAnimBoolName, true);
            _windBoss.Animator.speed = 0;
            _windBoss.CapsuleCollider.enabled = false;
            _windBoss.transform.position = new Vector3(_windBoss.transform.position.x, _windBoss.transform.position.y, 10);

            StateTimer = .15f;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer > 0)
            {
                Rb.linearVelocity = new Vector2(0, 10);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}