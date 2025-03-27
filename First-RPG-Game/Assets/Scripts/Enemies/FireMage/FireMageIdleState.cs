using UnityEngine;

namespace Enemies.FireMage
{
    public class FireMageIdleState : FireMageGroundedState
    {
        public FireMageIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMage _fireMage) : base(enemyBase, stateMachine, animBoolName, _fireMage)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = fireMage.idleTime;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
            {
                //Debug.Log("Change state");
                StateMachine.ChangeState(fireMage.MoveState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    

    }
}
