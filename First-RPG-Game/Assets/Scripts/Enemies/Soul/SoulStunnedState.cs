using Enemies.Archer;
using UnityEngine;

namespace Enemies.Soul
{
    public class SoulStunnedState : EnemyState
    {
        private EnemySoul _soul;

        public SoulStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySoul enemy) :
            base(enemyBase, stateMachine, animBoolName)
        {
            _soul = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            _soul.FX.InvokeRepeating("RedColorBlink", 0, .1f);

            StateTimer = _soul.stunDuration;

            Rb.linearVelocity = new Vector2(-_soul.FacingDir * _soul.stunDirection.x, _soul.stunDirection.y);
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
                StateMachine.ChangeState(_soul.IdleState);
        }

        public override void Exit()
        {
            base.Exit();

            _soul.FX.Invoke("CancelColorChange", 0);
        }
    }
}