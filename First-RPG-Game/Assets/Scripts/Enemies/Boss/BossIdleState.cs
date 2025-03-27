using UnityEngine;

namespace Enemies.Boss
{
    public class BossIdleState : BossGroundedState
    {
        public BossIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyBoss _boss) : base(enemyBase, stateMachine, animBoolName, _boss)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StateTimer = boss.idleTime;
        }


        public override void Update()
        {

            base.Update();

            if (StateTimer <= 0)
            {
                //Debug.Log("Change state");
                StateMachine.ChangeState(boss.MoveState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
}
