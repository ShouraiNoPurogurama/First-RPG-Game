using UnityEngine;

namespace Enemies.FireWorm
{
    public class FireWormDeadState : EnemyState
    {
        private readonly EnemyFireWorm _fireWorm;
        private bool _hasFallen;

        public FireWormDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireWorm fireWorm) : base(enemyBase, stateMachine, animBoolName)
        {
            _fireWorm = fireWorm;
        }

        public override void Enter()
        {
            _fireWorm.Animator.SetTrigger("Dead");
            //Debug.Log("DEAD -----------------------" + _fireWorm.LastAnimBoolName);
            base.Enter();

            //_fireWorm.Animator.SetBool(_fireWorm.LastAnimBoolName, true);
            //_fireWorm.Animator.speed = 0;
            //_fireWorm.CapsuleCollider.enabled = false;
            //_fireWorm.transform.position = new Vector3(_fireWorm.transform.position.x, _fireWorm.transform.position.y, 10);

            StateTimer = _fireWorm.Animator.GetCurrentAnimatorStateInfo(0).length;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
            {
                _fireWorm.Animator.speed = 0;
                _fireWorm.CapsuleCollider.enabled = false;
                //Rb.velocity = new Vector2(0, -10); // Rơi xuống
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
