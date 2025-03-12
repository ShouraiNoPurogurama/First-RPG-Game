using UnityEngine;

namespace Enemies.FireQueen
{
    public class FireQueenDeadState : EnemyState
    {
        private readonly EnemyFireQueen _fireQueen;
        private bool _hasFallen;

        public FireQueenDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireQueen FireQueen) : base(enemyBase, stateMachine, animBoolName)
        {
            _fireQueen = FireQueen;
        }

        public override void Enter()
        {
            _fireQueen.Animator.SetTrigger("Dead");
            //Debug.Log("DEAD -----------------------" + _fireQueen.LastAnimBoolName);
            base.Enter();

            //_fireQueen.Animator.SetBool(_fireQueen.LastAnimBoolName, true);
            //_fireQueen.Animator.speed = 0;
            //_fireQueen.CapsuleCollider.enabled = false;
            //_fireQueen.transform.position = new Vector3(_fireQueen.transform.position.x, _fireQueen.transform.position.y, 10);

            StateTimer = _fireQueen.Animator.GetCurrentAnimatorStateInfo(0).length;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
            {
                _fireQueen.Animator.speed = 0;
                _fireQueen.CapsuleCollider.enabled = false;
                //Rb.velocity = new Vector2(0, -10); // Rơi xuống
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
