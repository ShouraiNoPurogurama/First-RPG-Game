using Enemies.Frogger;
using UnityEngine;

namespace Enemies.Frogger
{
    public class FroggerStunnedState : EnemyState
    {
        private EnemyFrogger _Frogger;

        public FroggerStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFrogger enemy) :
            base(enemyBase, stateMachine, animBoolName)
        {
            _Frogger = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            _Frogger.FX.InvokeRepeating("RedColorBlink", 0, .1f);

            StateTimer = _Frogger.stunDuration;

            Rb.linearVelocity = new Vector2(-_Frogger.FacingDir * _Frogger.stunDirection.x, _Frogger.stunDirection.y);
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
                StateMachine.ChangeState(_Frogger.IdleState);
        }

        public override void Exit()
        {
            base.Exit();

            _Frogger.FX.Invoke("CancelColorChange", 0);
        }
    }
}