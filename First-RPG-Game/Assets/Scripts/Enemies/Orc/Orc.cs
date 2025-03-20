using Enemies;
using Enemies.Skeleton;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Enemies.Orc
{
    public class Orc : Enemy
    {
        public OrcMoveState MoveState { get; private set; }
        protected override void Awake()
        {
            base.Awake();

            MoveState = new OrcMoveState(this, StateMachine, "Move", this);

            counterImage.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }
    }

}
