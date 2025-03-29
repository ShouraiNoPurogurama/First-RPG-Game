using Audio;
using UnityEngine;

namespace Enemies.FireMage
{
    public class FireMageDeadState : EnemyState
    {
        private readonly EnemyFireMage _fireMage;
        private bool _hasFallen;

        public FireMageDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFireMage fireMage) : base(enemyBase, stateMachine, animBoolName)
        {
            _fireMage = fireMage;
        }

        public override void Enter()
        {
            _fireMage.Animator.SetTrigger("Dead");
            SoundManager.PlaySFX("FireMage", 1, true);
            //Debug.Log("DEAD -----------------------" + _fireMage.LastAnimBoolName);
            base.Enter();

            //_fireMage.Animator.SetBool(_fireMage.LastAnimBoolName, true);
            //_fireMage.Animator.speed = 0;
            //_fireMage.CapsuleCollider.enabled = false;
            //_fireMage.transform.position = new Vector3(_fireMage.transform.position.x, _fireMage.transform.position.y, 10);

            StateTimer = _fireMage.Animator.GetCurrentAnimatorStateInfo(0).length + .5f;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
            {
                _fireMage.Animator.speed = 0;
                _fireMage.CapsuleCollider.enabled = false;
                //Rb.velocity = new Vector2(0, -10); // Rơi xuống
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
