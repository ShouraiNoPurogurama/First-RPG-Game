using UnityEngine;

namespace Enemies.Infantryman
{
    public class EnemyInfantryman : Enemy
    {
        public EnemyInfantrymanIdleState IdleState { get; private set; }
        public EnemyInfantrymanMoveState MoveState { get; private set; }
        public EnemyInfantrymanAttackState AttackState { get; private set; }
        public EnemyInfantrymanBattleState BattleState { get; private set; }    

        //public EnemySwordmanBattleState BattleState { get; private set; }
        //public EnemySwordmanAttackState AttackState { get; private set; }
        //    //public enemyswordmanstunnedstate stunnedstate { get; private set; }
        //    //public enemyswordmandeadstate deadstate { get; private set; }

        //    protected override void Awake()
        //    {
        //        base.Awake();

        //        IdleState = new EnemySwordmanIdleState(this, StateMachine, "Idle",this);
        //        //MoveState = new EnemySwordmanMoveState(this, StateMachine, "Move", this);
        //        //BattleState = new EnemySwordmanBattleState(this, StateMachine, "Move", this);
        //        //AttackState = new EnemySwordmanAttackState(this, StateMachine, "Attack", this);
        //        //StunnedState = new EnemySwordmanStunnedState(this, StateMachine, "Stunned", this);
        //        //DeadState = new EnemySwordmanDeadState(this, StateMachine, "Idle", this);

        //        counterImage.SetActive(false);
        //    }

        //    protected override void Start()
        //    {
        //        base.Start();
        //        StateMachine.Initialize(IdleState);
        //    }

        //    protected override void Update()
        //    {
        //        base.Update();
        //    }

        //    //public override bool IsCanBeStunned(bool forceStun)
        //    //{
        //    //    if (base.IsCanBeStunned(forceStun))
        //    //    {
        //    //        StateMachine.ChangeState(StunnedState);
        //    //        return true;
        //    //    }

        //    //    return false;
        //    //}

        //    public override void Flip()
        //    {
        //        if (IsBusy)
        //            return;

        //        base.Flip();

        //        StartCoroutine("BusyFor", .3f);
        //    }

        //    //public override void Die()
        //    //{
        //    //    base.Die();
        //    //    StateMachine.ChangeState(DeadState);
        //    //}
        protected override void Awake()
        {
            base.Awake();

            IdleState = new EnemyInfantrymanIdleState(this, StateMachine, "Idle", this);
            MoveState = new EnemyInfantrymanMoveState(this, StateMachine, "Move", this);
            BattleState = new EnemyInfantrymanBattleState(this, StateMachine, "Move", this);
            AttackState = new EnemyInfantrymanAttackState(this, StateMachine,"Attack", this);
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
    }
}
