using Enemies;
using Enemies.Skeleton;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Enemies.Orc
{
    public class Orc : Enemy
    {
        #region States
        public OrcIdleState idleState { get; private set; }
        public OrcMoveState moveState { get; private set; }
        public OrcBattleState battleState { get; private set; }
        public OrcAttackState attackState { get; private set; }
        public OrcStunnedState stunnedState { get; private set; }
        public OrcDeadState deadState { get; private set; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            idleState = new OrcIdleState(this, StateMachine, "Idle", this);
            moveState = new OrcMoveState(this, StateMachine, "Move", this);
            battleState = new OrcBattleState(this, StateMachine, "Move", this);
            attackState = new OrcAttackState(this, StateMachine, "Attack", this);
            stunnedState = new OrcStunnedState(this, StateMachine, "Stunned", this);
            deadState = new OrcDeadState(this, StateMachine, "Dead", this);

            counterImage.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(idleState);
        }

        protected override void Update()
        {
            base.Update();
        }
        public override bool IsCanBeStunned(bool forceStun)
        {
            if (base.IsCanBeStunned(forceStun))
            {
                StateMachine.ChangeState(stunnedState);
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
            StateMachine.ChangeState(deadState);
            base.Die();
        }
    }

}
