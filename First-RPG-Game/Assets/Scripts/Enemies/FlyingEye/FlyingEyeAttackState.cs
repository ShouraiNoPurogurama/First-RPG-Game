using UnityEngine;

namespace Enemies.FlyingEye
{
    public class FlyingEyeAttackState : EnemyState
    {
        protected FlyingEye flyingEye;
        private Transform playerTransform;
        private int _moveDir;

        public FlyingEyeAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, FlyingEye flyingEye) : base(enemyBase, stateMachine, animBoolName)
        {
            this.flyingEye = flyingEye;
        }

        public override void Enter()
        {
            base.Enter();
            playerTransform = GameObject.Find("Player")?.transform;
        }
        public override void Update()
        {
            base.Update();
            if (playerTransform == null) return; 

            float distanceToPlayer = Vector2.Distance(flyingEye.transform.position, playerTransform.position);
            if (distanceToPlayer > flyingEye.attackCheckRadius)
            {
                StateMachine.ChangeState(flyingEye.IdleState); 
            }
        
        }
        public override void Exit() { base.Exit(); }
    }
}
