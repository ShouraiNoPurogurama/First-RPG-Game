using UnityEngine;

namespace Enemies.DarkBoss
{
    public class DarkBossDeadState : EnemyState
    {
        private DarkBoss _darkBoss;
        public DarkBossDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, DarkBoss darkBoss) : base(enemyBase, stateMachine, animBoolName)
        {
            _darkBoss = darkBoss;
        }

        public override void Enter()
        {
            base.Enter();
            _darkBoss.Animator.SetBool(_darkBoss.LastAnimBoolName, true);
            _darkBoss.Animator.speed = 0;
            _darkBoss.CapsuleCollider.enabled = false;
            _darkBoss.transform.position = new Vector3(_darkBoss.transform.position.x, _darkBoss.transform.position.y, 10);
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


    }
}
