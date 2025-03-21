using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossStunnedState : EnemyState
    {
        private WindBoss _windBoss;

        public WindBossStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) :
            base(enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }

        public override void Enter()
        {
            base.Enter();

            _windBoss.FX.InvokeRepeating("RedColorBlink", 0, .1f);

            StateTimer = _windBoss.stunDuration;

            Rb.linearVelocity = new Vector2(-_windBoss.FacingDir * _windBoss.stunDirection.x, _windBoss.stunDirection.y);
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
                StateMachine.ChangeState(_windBoss.IdleState);
        }

        public override void Exit()
        {
            base.Exit();

            _windBoss.FX.Invoke("CancelColorChange", 0);
        }
    }
}