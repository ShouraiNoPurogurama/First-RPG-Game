using UnityEngine;

namespace Enemies.FireWorm
{
    public class EnemyFireWorm : Enemy
    {
        #region States
        public FireWormIdleState IdleState { get; private set; }
        public FireWormMoveState MoveState { get; private set; }
        public FireWormBattleState BattleState { get; private set; }
        public FireWormAttackState AttackState { get; private set; }
        public FireWormDeadState DeadState { get; private set; }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            IdleState = new FireWormIdleState(this, StateMachine, "Idle", this);
            MoveState = new FireWormMoveState(this, StateMachine, "Move", this);
            BattleState = new FireWormBattleState(this, StateMachine, "Move", this);
            AttackState = new FireWormAttackState(this, StateMachine, "Attack", this);
            DeadState = new FireWormDeadState(this, StateMachine, "Dead", this);

            if (attackCheck == null)
            {
                attackCheck = transform.Find("AttackCheck");
                if (attackCheck == null)
                {
                    Debug.LogError("attackCheck chưa được gán trong Inspector và không tìm thấy trong Transform.");
                }
            }
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
            //Debug.Log("day roiiiiiiiiiiii");
            //transform.position = new Vector3(transform.position.x, transform.position.y, 10);
            StateMachine.ChangeState(DeadState);
            base.Die();
        }
        

    }
}

