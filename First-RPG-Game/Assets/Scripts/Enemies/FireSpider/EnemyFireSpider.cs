using UnityEngine;

namespace Enemies.FireSpider
{
    public class EnemyFireSpider : Enemy
    {
        #region States
        public FireSpiderIdleState IdleState { get; private set; }
        public FireSpiderMoveState MoveState { get; private set; }
        public FireSpiderBattleState BattleState { get; private set; }
        public FireSpiderAttackState AttackState { get; private set; }
        public FireSpiderDeadState DeadState { get; private set; }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            IdleState = new FireSpiderIdleState(this, StateMachine, "Idle", this);
            MoveState = new FireSpiderMoveState(this, StateMachine, "Move", this);
            BattleState = new FireSpiderBattleState(this, StateMachine, "Move", this);
            AttackState = new FireSpiderAttackState(this, StateMachine, "Attack", this);
            DeadState = new FireSpiderDeadState(this, StateMachine, "Dead", this);
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

