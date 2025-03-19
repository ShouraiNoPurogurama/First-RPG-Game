using Enemies.Frogger;
using UnityEngine;

namespace Enemies.Frogger
{
    public class FroggerDeadState : EnemyState
    {
        private EnemyFrogger _Frogger;

        public FroggerDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFrogger enemy) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _Frogger = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            _Frogger.Animator.SetBool(_Frogger.LastAnimBoolName, true);
            _Frogger.Animator.speed = 0;
            _Frogger.CapsuleCollider.enabled = false;
            _Frogger.transform.position = new Vector3(_Frogger.transform.position.x, _Frogger.transform.position.y, 10);

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