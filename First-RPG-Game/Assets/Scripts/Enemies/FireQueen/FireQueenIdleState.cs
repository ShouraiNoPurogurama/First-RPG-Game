using UnityEngine;

namespace Enemies.FireQueen
{
    public class FireQueenIdleState : FireQueenGroundedState
    {
        public FireQueenIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireQueen _fireQueen) : base(enemyBase, stateMachine, animBoolName, _fireQueen)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = fireQueen.idleTime;
        }

        public override void Update()
        {

            base.Update();

            if (StateTimer <= 0)
            {
                //Debug.Log("Change state");
                StateMachine.ChangeState(fireQueen.MoveState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    

    }
}
