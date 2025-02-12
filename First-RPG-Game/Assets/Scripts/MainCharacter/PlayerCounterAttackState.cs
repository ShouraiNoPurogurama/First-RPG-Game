using Enemies;
using UnityEngine;

namespace MainCharacter
{
    public class PlayerCounterAttackState : PlayerState
    {
        private bool _canCreatClone;
        private bool _successfulCounterAttack;
        
        public PlayerCounterAttackState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(
            stateMachine, player, animationBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _successfulCounterAttack = false;
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
                if (enemy is not null && enemy.IsCanBeStunned(false))
                {
                    enemy.Damage();
                    _successfulCounterAttack = true;
                    StateTimer = 100; //any value long enough
                    Player.Animator.SetBool("SuccessfulCounterAttack", true);
                    
                    if (_canCreatClone)
                    {
                        _canCreatClone = false;
                        Player.SkillManager.Clone.CreateCloneOnCounterAttack(enemy.transform);
                    }
                }
            }

            if (_successfulCounterAttack)
            {
                float newY = Mathf.MoveTowards(Player.transform.position.y, Player.transform.position.y + 10, 8f * Time.deltaTime);
                Player.transform.position = new Vector2(Player.transform.position.x, newY);
            }
            
            if (StateTimer < 0 || TriggerCalled)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        public override void Exit()
        {
            _canCreatClone = true;
            base.Exit();
        }
    }
}