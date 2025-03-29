using UnityEngine;

namespace Enemies.FlyingEye
{
    public class FlyingEyeDeadState : EnemyState
    {
        protected FlyingEye flyingEye;
        public FlyingEyeDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, FlyingEye flyingEye) : base(enemyBase, stateMachine, animBoolName)
        {
            this.flyingEye = flyingEye;
        }
        public override void Enter()
        {
            base.Enter();

            flyingEye.Animator.SetBool(flyingEye.LastAnimBoolName, true);
            flyingEye.Animator.speed = 0;
            flyingEye.CapsuleCollider.enabled = false;
            flyingEye.transform.position = new Vector3(flyingEye.transform.position.x, flyingEye.transform.position.y, 10);

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
