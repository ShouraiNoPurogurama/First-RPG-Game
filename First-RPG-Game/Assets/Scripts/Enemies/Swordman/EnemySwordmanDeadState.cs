using UnityEngine;

namespace Enemies.Swordman
{
    public class EnemySwordmanDeadState : EnemyState
    {
        private readonly EnemySwordman _enemy;
        
        public EnemySwordmanDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySwordman enemy) : base(enemyBase, stateMachine, animBoolName)

        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            _enemy.Animator.SetBool(_enemy.LastAnimBoolName, true);
            _enemy.Animator.speed = 0;
            _enemy.CapsuleCollider.enabled = false;
            _enemy.transform.position = new Vector3(_enemy.transform.position.x, _enemy.transform.position.y, 10);

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
