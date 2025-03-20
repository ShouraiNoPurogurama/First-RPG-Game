using UnityEngine;

namespace Enemies.FireMiniMage
{
    public class FireMiniMageDeadState : EnemyState
    {
        private readonly EnemyFireMiniMage _FireMiniMage;
        private bool _hasFallen;

        public FireMiniMageDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMiniMage FireMiniMage) : base(enemyBase, stateMachine, animBoolName)
        {
            _FireMiniMage = FireMiniMage;
        }

        public override void Enter()
        {
            _FireMiniMage.Animator.SetTrigger("Dead");
            //Debug.Log("DEAD -----------------------" + _FireMiniMage.LastAnimBoolName);
            base.Enter();

            //_FireMiniMage.Animator.SetBool(_FireMiniMage.LastAnimBoolName, true);
            //_FireMiniMage.Animator.speed = 0;
            //_FireMiniMage.CapsuleCollider.enabled = false;
            //_FireMiniMage.transform.position = new Vector3(_FireMiniMage.transform.position.x, _FireMiniMage.transform.position.y, 10);

            StateTimer = _FireMiniMage.Animator.GetCurrentAnimatorStateInfo(0).length + .5f;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
            {
                _FireMiniMage.Animator.speed = 0;
                _FireMiniMage.CapsuleCollider.enabled = false;
                //Rb.velocity = new Vector2(0, -10); // Rơi xuống
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
