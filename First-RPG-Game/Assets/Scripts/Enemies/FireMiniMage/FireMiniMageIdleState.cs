using UnityEngine;

namespace Enemies.FireMiniMage
{
    public class FireMiniMageIdleState : FireMiniMageGroundedState
    {
        public FireMiniMageIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMiniMage _FireMiniMage) : base(enemyBase, stateMachine, animBoolName, _FireMiniMage)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = FireMiniMage.idleTime;
        }
        
        public override void Update()
        {

            base.Update();

            if (StateTimer <= 0)
            {
                //Debug.Log("Change state");
                StateMachine.ChangeState(FireMiniMage.MoveState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    

    }
}
