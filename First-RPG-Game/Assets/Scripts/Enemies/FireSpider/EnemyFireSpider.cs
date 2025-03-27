using UnityEngine;
using System.Collections;

namespace Enemies.FireSpider
{
    public class EnemyFireSpider : Enemy
    {
        [SerializeField] private int healAmount = 10;
        [SerializeField] private float healInterval = 3f;
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
            InvokeRepeating(nameof(HealOverTime), healInterval, healInterval);
        }


        protected override void Update()
        {
            base.Update();
        }
        private void HealOverTime()
        {

            //Debug.Log("heal------------------------");
            if (Stats.currentHp <= 0) return; // Nếu đã chết, không hồi máu nữa
            //Debug.Log("heal heal heal ------------------------");
            Stats.RecoverHPBy(healAmount);
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

