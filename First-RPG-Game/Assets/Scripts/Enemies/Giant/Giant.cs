using UnityEngine;

namespace Enemies.Giant
{
    public class Giant : Enemy
    {
        public GiantIdleState IdleState { get; private set; }
        public GiantMoveState MoveState { get; private set; }
        public GiantBattleState BattleState { get; private set; }

        public GiantAttackState AttackState { get; private set; }
        public GiantDeadState DeadState { get; private set; }
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        protected override void Awake()
        {
            base.Awake();
            IdleState = new GiantIdleState(this, StateMachine, "Idle", this);
            MoveState = new GiantMoveState(this, StateMachine, "Move", this);
            BattleState = new GiantBattleState(this, StateMachine, "Move", this);
            AttackState = new GiantAttackState(this, StateMachine, "Attack", this);
            DeadState = new GiantDeadState(this, StateMachine, "Idle", this);
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
