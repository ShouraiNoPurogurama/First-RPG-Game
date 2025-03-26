using UnityEngine;

namespace Enemies.FireSnake
{
    public class FireSnakeIdleState : FireSnakeGroundedState
    {
        public FireSnakeIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSnake _fireSnake) : base(enemyBase, stateMachine, animBoolName, _fireSnake)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = fireSnake.idleTime;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
            {
                //Debug.Log("Change state");
                StateMachine.ChangeState(fireSnake.MoveState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    

    }
}
