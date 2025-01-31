using Enemies;
using UnityEngine;

namespace MainCharacter
{
    public class PlayerCounterAttackState : PlayerState
    {
        public PlayerCounterAttackState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(
            stateMachine, player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StateTimer = Player.counterAttackDuration;
            Player.Animator.SetBool("SuccessfulCounterAttack", false);
        }

        public override void Update()
        {
            base.Update();

            Player.SetZeroVelocity();
            
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Player.attackCheck.position, Player.attackCheckRadius);

            foreach (var hit in colliders)
            {
                var enemy = hit.GetComponent<Enemy>();
                if (enemy is not null && enemy.IsCanBeStunned())
                {
                    StateTimer = 100; // any value long enough
                    Player.Animator.SetBool("SuccessfulCounterAttack", true);
                }
            }

            if (StateTimer < 0 || TriggerCalled)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}