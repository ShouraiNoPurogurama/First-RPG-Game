using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Enemies.Orc
{
    public class OrcDeadState : EnemyState
    {
        private Orc Orc;
        public OrcDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Orc orc) : base(enemyBase, stateMachine, animBoolName)
        {
            this.Orc = orc;
        }
        public override void Enter()
        {
            Orc.Animator.SetTrigger("Dead");
            base.Enter();

            //Orc.Animator.SetBool(Orc.LastAnimBoolName, true);
            //Orc.Animator.speed = 0;
            //Orc.CapsuleCollider.enabled = false;
            //Orc.transform.position = new Vector3(Orc.transform.position.x, Orc.transform.position.y, 10);

            //StateTimer = .15f;
            StateTimer = Orc.Animator.GetCurrentAnimatorStateInfo(0).length;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer > 0)
            {
                //Rb.linearVelocity = new Vector2(0, 10);
                Orc.Animator.speed = 0;
                Orc.CapsuleCollider.enabled = false;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
