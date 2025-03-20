using UnityEngine;

namespace Enemies.Skeleton
{
    public class SkeletonStunnedState : EnemyState
    {
        private EnemySkeleton _skeleton;
        
        public SkeletonStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton skeleton) : base(enemyBase, stateMachine, animBoolName)
        {
            _skeleton = skeleton;
        }
        
        public override void Enter()
        {
            base.Enter();

            _skeleton.FX.InvokeRepeating("RedColorBlink", 0, .1f);
            
            StateTimer = _skeleton.stunDuration;
            
            Rb.linearVelocity = new Vector2(-_skeleton.FacingDir * _skeleton.stunDirection.x, _skeleton.stunDirection.y);
        }

        public override void Update()
        {
            base.Update();

            if(StateTimer <= 0)
                StateMachine.ChangeState(_skeleton.IdleState);
        }

        public override void Exit()
        {
            base.Exit();

            _skeleton.FX.Invoke("CancelColorChange", 0);
        }
    }
}