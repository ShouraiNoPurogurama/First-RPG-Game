using UnityEngine;

namespace Enemies.FireSpider
{
    public class FireSpiderDeadState : EnemyState
    {
        private readonly EnemyFireSpider _fireSpider;
        private bool _hasFallen;

        public FireSpiderDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSpider FireSpider) : base(enemyBase, stateMachine, animBoolName)
        {
            _fireSpider = FireSpider;
        }

        public override void Enter()
        {
            _fireSpider.Animator.SetTrigger("Dead");
            //Debug.Log("DEAD -----------------------" + _fireSpider.LastAnimBoolName);
            base.Enter();
            StateTimer = _fireSpider.Animator.GetCurrentAnimatorStateInfo(0).length;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
            {
                _fireSpider.Animator.speed = 0;
                _fireSpider.CapsuleCollider.enabled = false;
                //Rb.velocity = new Vector2(0, -10); // Rơi xuống
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
