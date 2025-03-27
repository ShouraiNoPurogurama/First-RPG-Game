using UnityEngine;

namespace Enemies.FireWorm
{
    public class FireWormIdleState : FireWormGroundedState
    {
        public FireWormIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireWorm _fireWorm) : base(enemyBase, stateMachine, animBoolName, _fireWorm)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = fireWorm.idleTime;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
            {
                //Debug.Log("Change state");
                StateMachine.ChangeState(fireWorm.MoveState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    

    }
}
