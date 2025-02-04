using UnityEngine;

namespace Enemies.Skeleton
{
    public class SkeletonGroundedState : EnemyState
    {
        protected readonly EnemySkeleton Skeleton;

        private Transform _player;

        protected SkeletonGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton skeleton) : base(enemyBase, stateMachine, animBoolName)
        {
            Skeleton = skeleton;
        }

        public override void Enter()
        {
            base.Enter();

            _player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();
            
            if (Skeleton.IsPlayerDetected() || Vector2.Distance(Skeleton.transform.position, _player.position) < 2)
            {
                StateMachine.ChangeState(Skeleton.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
