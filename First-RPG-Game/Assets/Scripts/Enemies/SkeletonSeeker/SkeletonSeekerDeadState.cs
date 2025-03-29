using UnityEngine;

namespace Enemies.SkeletonSeeker
{
    public class SkeletonSeekerDeadState : EnemyState
    {
        private SkeletonSeeker _skeletonSeeker;
        public SkeletonSeekerDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, SkeletonSeeker skeletonSeeker) : base(enemyBase, stateMachine, animBoolName)
        {
            _skeletonSeeker = skeletonSeeker;
        }

        public override void Enter()
        {
            base.Enter();

            _skeletonSeeker.Animator.SetBool(_skeletonSeeker.LastAnimBoolName, true);
            _skeletonSeeker.Animator.speed = 0;
            _skeletonSeeker.CapsuleCollider.enabled = false;
            _skeletonSeeker.transform.position = new Vector3(_skeletonSeeker.transform.position.x, _skeletonSeeker.transform.position.y, 10);

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
