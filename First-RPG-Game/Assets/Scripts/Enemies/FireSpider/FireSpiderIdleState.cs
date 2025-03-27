using UnityEngine;

namespace Enemies.FireSpider
{
    public class FireSpiderIdleState : FireSpiderGroundedState
    {
        public FireSpiderIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSpider _fireSpider) : base(enemyBase, stateMachine, animBoolName, _fireSpider)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = fireSpider.idleTime;
        }
        
        public override void Update()
        {

            base.Update();

            if (StateTimer <= 0)
            {
                //Debug.Log("Change state");
                StateMachine.ChangeState(fireSpider.MoveState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    

    }
}
