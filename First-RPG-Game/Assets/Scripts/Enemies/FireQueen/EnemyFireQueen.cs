using UnityEngine;

namespace Enemies.FireQueen
{
    public class EnemyFireQueen : Enemy
    {
        [SerializeField] public int attackSfxIndex = 0;
        #region States
        public FireQueenIdleState IdleState { get; private set; }
        public FireQueenMoveState MoveState { get; private set; }
        public FireQueenBattleState BattleState { get; private set; }
        public FireQueenAttackState AttackState { get; private set; }
        public FireQueenDeadState DeadState { get; private set; }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            IdleState = new FireQueenIdleState(this, StateMachine, "Idle", this);
            MoveState = new FireQueenMoveState(this, StateMachine, "Move", this);
            BattleState = new FireQueenBattleState(this, StateMachine, "Move", this);
            AttackState = new FireQueenAttackState(this, StateMachine, "Attack", this);
            DeadState = new FireQueenDeadState(this, StateMachine, "Dead", this);
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
            //Debug.Log("IsBusy: " + IsBusy);

            if (IsBusy)
                return;

            base.Flip();

            StartCoroutine("BusyFor", .3f);
        }

        public override void Die()
        {
            StateMachine.ChangeState(DeadState);
            base.Die();
        }


    }
}

