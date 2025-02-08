using UnityEngine;

namespace MainCharacter
{
    public class PlayerBlackHoleState : PlayerState
    {
        private float _flyTime = .3f;
        private bool _skillUsed;
        private float _defaultGravity;
        
        public PlayerBlackHoleState(PlayerStateMachine stateMachine, Player player, string animationBoolName) : base(stateMachine, player, animationBoolName)
        {
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
        }

        public override void Enter()
        {
            base.Enter();

            _defaultGravity = Player.Rb.gravityScale;

            _skillUsed = false;
            StateTimer = _flyTime;
            Rb.gravityScale = 0;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer > 0)
            {
                Rb.linearVelocity = new Vector2(0, 15);
            }

            if (StateTimer < 0)
            {
                Rb.linearVelocity = new Vector2(0, -.1f);

                if (!_skillUsed)
                {
                    if (Player.SkillManager.BlackHole.CanUseSkill())
                    {
                        _skillUsed = true;
                    }
                }
            }

            if (Player.SkillManager.BlackHole.SkillFinished())
            {
                StateMachine.ChangeState(Player.AirState);
            }
        }

        public override void Exit()
        {
            base.Exit();

            Player.Rb.gravityScale = _defaultGravity;
            Player.SetTransparent(false);
        }
        
        //Exit state in blackhole skills controller when all the attacks is over
    }
}
