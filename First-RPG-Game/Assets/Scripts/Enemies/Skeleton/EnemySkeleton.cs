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
        public SkeletonDeadState DeadState { get; private set; }
        

        protected override void Awake()
        {
            base.Awake();

            IdleState = new SkeletonIdleState(this, StateMachine, "Idle", this);
            MoveState = new SkeletonMoveState(this, StateMachine, "Move", this);
            BattleState = new SkeletonBattleState(this, StateMachine, "Move", this);
            AttackState = new SkeletonAttackState(this, StateMachine, "Attack", this);
            StunnedState = new SkeletonStunnedState(this, StateMachine, "Stunned", this);
            DeadState = new SkeletonDeadState(this, StateMachine, "Idle", this);

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

        public override bool IsCanBeStunned(bool forceStun)
        {
            if (base.IsCanBeStunned(forceStun))
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
            
            StartCoroutine("BusyFor", .3f);
        }

        public override void Die()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 10);
            StateMachine.ChangeState(DeadState);
            base.Die();
        }
    }
}