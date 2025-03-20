using UnityEngine;

namespace Enemies.FireSnake
{
    public class FireSnakeDeadState : EnemyState
    {
        private readonly EnemyFireSnake _fireSnake;
        private bool _hasFallen;

        public FireSnakeDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireSnake FireSnake) : base(enemyBase, stateMachine, animBoolName)
        {
            _fireSnake = FireSnake;
        }

        public override void Enter()
        {
            _fireSnake.Animator.SetTrigger("Dead");
            //Debug.Log("DEAD -----------------------" + _fireSnake.LastAnimBoolName);
            base.Enter();
            StateTimer = _fireSnake.Animator.GetCurrentAnimatorStateInfo(0).length;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
            {
                _fireSnake.Animator.speed = 0;
                _fireSnake.CapsuleCollider.enabled = false;
                //Rb.velocity = new Vector2(0, -10); // Rơi xuống
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
