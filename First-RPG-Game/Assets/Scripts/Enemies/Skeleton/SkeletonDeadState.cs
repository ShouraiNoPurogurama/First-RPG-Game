using UnityEngine;

namespace Enemies.Skeleton
{
    public class SkeletonDeadState : EnemyState
    {
        private readonly EnemySkeleton _skeleton;
        
        public SkeletonDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton skeleton) : base(enemyBase, stateMachine, animBoolName)
        {
            _skeleton = skeleton;
        }

        public override void Enter()
        {
            base.Enter();

            _skeleton.Animator.SetBool(_skeleton.LastAnimBoolName, true);
            _skeleton.Animator.speed = 0;
            _skeleton.CapsuleCollider.enabled = false;
            _skeleton.transform.position = new Vector3(_skeleton.transform.position.x, _skeleton.transform.position.y, 10);

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
