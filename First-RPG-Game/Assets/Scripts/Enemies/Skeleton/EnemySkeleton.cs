using UnityEngine;

namespace Enemies.Skeleton
{
    public class EnemySkeleton : Enemy
    {
        public SkeletonIdleState IdleState { get; private set; }
        public SkeletonMoveState MoveState { get; private set; }
        public SkeletonBattleState BattleState { get; private set; }
        public SkeletonAttackState AttackState { get; private set; }
        public SkeletonStunnedState StunnedState { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            IdleState = new SkeletonIdleState(this, StateMachine, "Idle", this);
            MoveState = new SkeletonMoveState(this, StateMachine, "Move", this);
            BattleState = new SkeletonBattleState(this, StateMachine, "Move", this);
            AttackState = new SkeletonAttackState(this, StateMachine, "Attack", this);
            StunnedState = new SkeletonStunnedState(this, StateMachine, "Stunned", this);

            counterImage.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);
        }

        protected override void Update()
        {
            base.Update();
        }

        public override bool IsCanBeStunned()
        {
            if (base.IsCanBeStunned())
            {
                StateMachine.ChangeState(StunnedState);
                return true;
            }

            return false;
        }

        public override void Flip()
        {
            if (IsBusy)
                return;
            
            base.Flip();
            
            StartCoroutine("BusyFor", .4f);
        }
    }
}