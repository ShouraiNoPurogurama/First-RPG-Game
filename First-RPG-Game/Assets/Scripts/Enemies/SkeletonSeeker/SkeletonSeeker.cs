using UnityEngine;

namespace Enemies.SkeletonSeeker
{
    public class SkeletonSeeker : Enemy
    {
        public SkeletonSeekerAttackState AttackState { get; set; }
        public SkeletonSeekerBattleState BattleState { get; set; }
        public SkeletonSeekerDeadState DeadState { get; set; }
        public SkeletonSeekerGroundedState GroundedState { get; set; }
        public SkeletonSeekerMoveState MoveState { get; set; }
        public SkeletonSeekerIdleState IdleState { get; set; }
        public SkeletonSeekerSpawnState SpawnState { get; set; }
        public SkeletonSeekerStunnedState StunnedState { get; set; }


        protected override void Awake()
        {
            base.Awake();
            IdleState = new SkeletonSeekerIdleState(this, StateMachine, "Idle", this);
            MoveState = new SkeletonSeekerMoveState(this, StateMachine, "Move", this);
            BattleState = new SkeletonSeekerBattleState(this, StateMachine, "Move", this);
            AttackState = new SkeletonSeekerAttackState(this, StateMachine, "Attack", this);
            DeadState = new SkeletonSeekerDeadState(this, StateMachine, "Dead", this);
            SpawnState = new SkeletonSeekerSpawnState(this, StateMachine, "Spawn", this);
            StunnedState = new SkeletonSeekerStunnedState(this, StateMachine, "Stunned", this);
            counterImage.SetActive(false);

        }
        protected override void Start()
        {
            base.Start();

            StateMachine.Initialize(SpawnState);
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
