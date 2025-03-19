using Enemies;
using UnityEngine;

namespace MainCharacter
{
    public class PlayerCounterAttackState : PlayerState
    {
        private bool _canCreatClone;
        private bool _successfulCounterAttack;

        private float _flyUpTime = 0.3f;
        private float _elapsedTime;

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

            _elapsedTime = 0f;
        }

        public override void Update()
        {
            base.Update();

            Player.SetZeroVelocity();

            Collider2D[] colliders = Physics2D.OverlapCircleAll(Player.attackCheck.position, Player.attackCheckRadius);

            _elapsedTime += Time.deltaTime;

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<ArrowController>())
                {
                    hit.GetComponent<ArrowController>().FlipArrow();
                    SuccessfulCounterAttack();
                }
                
                var enemy = hit.GetComponent<Enemy>();
                if (enemy is not null && enemy.IsCanBeStunned(false))
                {
                    _successfulCounterAttack = true;
                    SuccessfulCounterAttack();

                    if (_canCreatClone)
                    {
                        _canCreatClone = false;
                        Player.SkillManager.Clone.CreateCloneOnCounterAttack(enemy.transform);
                    }
                }
            }

            if (_successfulCounterAttack)
            {
                if (_elapsedTime <= _flyUpTime)
                {
                    //Initial quick fly-up phase
                    float newY = Mathf.MoveTowards(Player.transform.position.y, Player.transform.position.y + 10,
                        10f * Time.deltaTime);
                    Player.transform.position = new Vector2(Player.transform.position.x, newY);
                }
                else
                {
                    StateMachine.ChangeState(Player.FallAfterAttackState);
                }
            }

            if (StateTimer < 0 || TriggerCalled)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        private void SuccessfulCounterAttack()
        {
            StateTimer = 100; //any value long enough
            Player.Animator.SetBool("SuccessfulCounterAttack", true);
        }

        public override void Exit()
        {
            _canCreatClone = true;

            base.Exit();
        }
    }
}