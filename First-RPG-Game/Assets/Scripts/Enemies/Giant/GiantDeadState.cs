using UnityEngine;

namespace Enemies.Giant
{
    public class GiantDeadState : EnemyState
    {
        public Giant Giant { get; private set; }
        public GiantDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Giant giant) : base(enemyBase, stateMachine, animBoolName)
        {
            Giant = giant;
        }

        public override void Enter()
        {
            base.Enter();
            Giant.Animator.SetBool(Giant.LastAnimBoolName, true);
            Giant.Animator.speed = 0;
            Giant.CapsuleCollider.enabled = false;
            Giant.transform.position = new Vector3(Giant.transform.position.x, Giant.transform.position.y, 10);
            StateTimer = .15f;
        }

        public override void Update()
        {
            base.Update();
            if (StateTimer > 0)
            {
                Rb.linearVelocity = new Vector2(0, 10);
            }
        }
    }
}
