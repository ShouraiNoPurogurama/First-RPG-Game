using UnityEngine;
namespace Enemies.Orc
{
    public class OrcStunnedState : EnemyState
    {
        private Orc Orc;

        public OrcStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Orc orc) : base(enemyBase, stateMachine, animBoolName)
        {
            Orc = orc;
        }

        public override void Enter()
        {
            base.Enter();

            Orc.FX.InvokeRepeating("RedColorBlink", 0, .1f);

            StateTimer = Orc.stunDuration;

            Rb.linearVelocity = new Vector2(-Orc.FacingDir * Orc.stunDirection.x, Orc.stunDirection.y);
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
                StateMachine.ChangeState(Orc.idleState);
        }

        public override void Exit()
        {
            base.Exit();

            Orc.FX.Invoke("CancelColorChange", 0);
        }
    }
}