using UnityEngine;

namespace Enemies.FlyingEye
{
    public class FlyingEyeIdleState : EnemyState
    {
        protected FlyingEye flyingEye;
        private int _moveDir;

        public FlyingEyeIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, FlyingEye flyingEye) : base(enemyBase, stateMachine, animBoolName)
        {
            this.flyingEye = flyingEye;
        }
        public override void Enter()
        {
            base.Enter();
        
        }
        public override void Update()
        {
            base.Update();

            if (flyingEye.IsPlayerInRange())
            {
                Transform playerTransform = GameObject.Find("Player").transform;

                int moveDir = playerTransform.position.x > flyingEye.transform.position.x ? 1 : -1;

                if (moveDir != flyingEye.FacingDir)
                {
                    flyingEye.Flip();
                }

                flyingEye.transform.position = Vector2.MoveTowards(
                    flyingEye.transform.position,
                    playerTransform.position,
                    flyingEye.moveSpeed * Time.deltaTime
                );

                if (Vector2.Distance(flyingEye.transform.position, playerTransform.position) <= 0.1f)
                {
                    flyingEye.SetVelocity(0, flyingEye.Rb.linearVelocity.y);
                    StateMachine.ChangeState(flyingEye.AttackState);
                }
            }
            else
            {
                // Continue normal movement
                flyingEye.SetVelocity(flyingEye.FacingDir * flyingEye.moveSpeed, flyingEye.Rb.linearVelocity.y);

                // Flip if a wall is detected
                if (!flyingEye.IsBusy && flyingEye.IsWallDetected())
                {
                    flyingEye.Flip();
                }
            }
        }
        public override void Exit() { base.Exit(); }
    }
}
