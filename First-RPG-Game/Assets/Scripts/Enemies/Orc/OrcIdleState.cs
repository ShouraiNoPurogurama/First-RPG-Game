using Enemies;
using Enemies.Orc;
using UnityEngine;

namespace Enemies.Orc
{
    public class OrcIdleState : OrcGroundedState
    {
        public OrcIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Orc orc) : base(enemyBase, stateMachine, animBoolName, orc)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = Orc.idleTime;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(Orc.moveState);
            }

        }
    }
}
