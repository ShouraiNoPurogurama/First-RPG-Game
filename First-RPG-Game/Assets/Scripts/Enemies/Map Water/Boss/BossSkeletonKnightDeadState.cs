using UnityEngine;

namespace Enemies.Map_Water.Boss
{
    public class BossSkeletonKnightDeadState : EnemyState
    {
        private BossSkeletonKnight enemy;

        public BossSkeletonKnightDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, BossSkeletonKnight enemy) : base(enemyBase, stateMachine, animBoolName)
        {
            this.enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            enemy.Animator.SetBool(enemy.LastAnimBoolName, true);
            enemy.Animator.speed = 0;
            enemy.CapsuleCollider.enabled = false;
            enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 10);

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
