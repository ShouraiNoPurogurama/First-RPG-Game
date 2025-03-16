using UnityEngine;

namespace Enemies.Archer
{
    public class ArcherDeadState : EnemyState
    {
        private EnemyArcher _archer;

        public ArcherDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher enemy) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _archer = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            _archer.Animator.SetBool(_archer.LastAnimBoolName, true);
            _archer.Animator.speed = 0;
            _archer.CapsuleCollider.enabled = false;
            _archer.transform.position = new Vector3(_archer.transform.position.x, _archer.transform.position.y, 10);

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