using UnityEngine;

namespace Enemies.Archer
{
    public class ArcherStunnedState : EnemyState
    {
        private EnemyArcher _archer;

        public ArcherStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher enemy) :
            base(enemyBase, stateMachine, animBoolName)
        {
            _archer = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            _archer.FX.InvokeRepeating("RedColorBlink", 0, .1f);

            StateTimer = _archer.stunDuration;

            Rb.linearVelocity = new Vector2(-_archer.FacingDir * _archer.stunDirection.x, _archer.stunDirection.y);
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
                StateMachine.ChangeState(_archer.IdleState);
        }

        public override void Exit()
        {
            base.Exit();

            _archer.FX.Invoke("CancelColorChange", 0);
        }
    }
}