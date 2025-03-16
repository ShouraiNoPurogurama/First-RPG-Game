using UnityEngine;

namespace Enemies.Map_Water.Magic_Skeleton
{
    public class MagicSkeletonGroundedState : EnemyState
    {
        protected Transform _player;
        protected Enemy_Magic_Skeleton MagicSkeleton;

        public MagicSkeletonGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Magic_Skeleton _magic_Skeleton_Enemy) : base(enemyBase, stateMachine, animBoolName)
        {
            MagicSkeleton = _magic_Skeleton_Enemy;
        }

        public override void Enter()
        {
            base.Enter();

            _player = GameObject.Find("Player").transform;
            //_player = PlayerManager.Instance.player.transform;

        }

        public override void Update()
        {
            base.Update();

            if (MagicSkeleton.IsPlayerDetected() || Vector2.Distance(MagicSkeleton.transform.position, _player.position) < 2)
            {
                Debug.Log("MagicSkeletonGroundedState: Move Magic Skeleton Battle State");
                Debug.Log("Distance: " + Vector2.Distance(MagicSkeleton.transform.position, _player.position));
                Debug.Log("Player Detected: " + MagicSkeleton.IsPlayerDetected().collider);
                StateMachine.ChangeState(MagicSkeleton.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
