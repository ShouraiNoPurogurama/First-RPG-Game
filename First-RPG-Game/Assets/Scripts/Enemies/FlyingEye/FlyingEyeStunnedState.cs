using UnityEngine;

namespace Enemies.FlyingEye
{
    public class FlyingEyeStunnedState : EnemyState
    {
        protected FlyingEye flyingEye;
        public FlyingEyeStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, FlyingEye flyingEye) : base(enemyBase, stateMachine, animBoolName)
        {
            this.flyingEye = flyingEye;
        }
        public override void Enter()
        {
            base.Enter();

            flyingEye.FX.InvokeRepeating("RedColorBlink", 0, .1f);

            StateTimer = flyingEye.stunDuration;

            Rb.linearVelocity = new Vector2(-flyingEye.FacingDir * flyingEye.stunDirection.x, flyingEye.stunDirection.y);
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
                StateMachine.ChangeState(flyingEye.IdleState);
        }

        public override void Exit()
        {
            base.Exit();

            flyingEye.FX.Invoke("CancelColorChange", 0);
        }
    }
}
