using Audio;
using UnityEngine;

namespace Enemies.Boss
{
    public class BossDeadState : EnemyState
    {
        private readonly EnemyBoss _boss;
        private bool _hasFallen;

        public BossDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyBoss boss) : base(enemyBase, stateMachine, animBoolName)
        {
            _boss = boss;
        }

        public override void Enter()
        {
            _boss.Animator.SetTrigger("Dead");
            //Debug.Log("DEAD -----------------------" + _boss.LastAnimBoolName);
            base.Enter();
            SoundManager.PlaySFX("FireBoss", 5, true);

            //_boss.Animator.SetBool(_boss.LastAnimBoolName, true);
            //_boss.Animator.speed = 0;
            //_boss.CapsuleCollider.enabled = false;
            //_boss.transform.position = new Vector3(_boss.transform.position.x, _boss.transform.position.y, 10);

            StateTimer = _boss.Animator.GetCurrentAnimatorStateInfo(0).length;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer <= 0)
            {
                _boss.Animator.speed = 0;
                _boss.CapsuleCollider.enabled = false;
                //Rb.velocity = new Vector2(0, -10); // Rơi xuống
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
