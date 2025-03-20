using UnityEngine;

namespace Enemies.FireSnake
{
    public class EnemyFireSnake : Enemy
    {
        #region States
        public FireSnakeIdleState IdleState { get; private set; }
        public FireSnakeMoveState MoveState { get; private set; }
        public FireSnakeBattleState BattleState { get; private set; }
        public FireSnakeAttackState AttackState { get; private set; }
        public FireSnakeDeadState DeadState { get; private set; }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            IdleState = new FireSnakeIdleState(this, StateMachine, "Idle", this);
            MoveState = new FireSnakeMoveState(this, StateMachine, "Idle", this);
            BattleState = new FireSnakeBattleState(this, StateMachine, "Idle", this);
            AttackState = new FireSnakeAttackState(this, StateMachine, "Attack", this);
            DeadState = new FireSnakeDeadState(this, StateMachine, "Dead", this);
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

