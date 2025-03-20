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
        #endregion

        protected override void Awake()
        {
            base.Awake();

            idleState = new OrcIdleState(this, StateMachine, "Idle", this);
            moveState = new OrcMoveState(this, StateMachine, "Move", this);

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
    }

}
