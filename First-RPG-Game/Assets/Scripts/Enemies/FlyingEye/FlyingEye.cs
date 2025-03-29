using UnityEngine;

namespace Enemies.FlyingEye
{
    public class FlyingEye : Enemy
    {
        [Header("Find Player")]
        public Transform playerCheck;

        public float playerCheckRadius;
        public LayerMask playerLayer;
        public FlyingEyeAttackState AttackState { get; set; }
        public FlyingEyeDeadState DeadState { get; set; }
        public FlyingEyeIdleState IdleState { get; set; }
        public FlyingEyeStunnedState StunnedState { get; set; }

        protected override void Awake()
        {
            base.Awake();
            IdleState = new FlyingEyeIdleState(this, StateMachine, "Idle", this);
            AttackState = new FlyingEyeAttackState(this, StateMachine, "Attack", this);
            DeadState = new FlyingEyeDeadState(this, StateMachine, "Dead", this);
            StunnedState = new FlyingEyeStunnedState(this, StateMachine, "Stunned", this);
        }
        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);
        }
        public override void Flip()
        {
            if (IsBusy)
                return;

            base.Flip();

            StartCoroutine("BusyFor", .3f);
        }
        public bool IsPlayerInRange()
        {
            return Physics2D.OverlapCircle(playerCheck.position, playerCheckRadius, playerLayer);
        }
        private void OnDrawGizmosSelected()
        {
            // Draw a circle in the scene view to visualize the detection range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerCheck.position, playerCheckRadius);
        }
        public override void Die()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 10);
            StateMachine.ChangeState(DeadState);
            base.Die();
        }

        public override bool IsCanBeStunned(bool forceStun)
        {
            if (base.IsCanBeStunned(forceStun))
            {
                StateMachine.ChangeState(StunnedState);
                return true;
            }

            return false;
        }
    }
}
