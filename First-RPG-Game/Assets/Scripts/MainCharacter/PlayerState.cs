using UnityEngine;

namespace MainCharacter
{
    public class PlayerState
    {
        protected PlayerStateMachine StateMachine;
        protected Player Player;

        protected Rigidbody2D Rb;

        protected float xInput;
        protected float yInput;
        private readonly string _animationBoolName;
    
        //Length of state animation
        protected float StateTimer;
        protected bool TriggerCalled;

        public PlayerState(PlayerStateMachine stateMachine, Player player, string animationBoolName)
        {
            StateMachine = stateMachine;
            Player = player;
            _animationBoolName = animationBoolName;
        }
        
        public virtual void Enter()
        {
            Player.Animator.SetBool(_animationBoolName, true);
            Rb = Player.Rb;
            TriggerCalled = false;
        }

        public virtual void Update()
        {
            StateTimer -= Time.deltaTime;
        
            xInput = Input.GetAxisRaw("Horizontal");
            yInput = Input.GetAxisRaw("Vertical");
        
            Player.Animator.SetFloat("yVelocity", Rb.linearVelocity.y);
        }

        public virtual void Exit()
        {
            Player.Animator.SetBool(_animationBoolName, false);
        }

        public virtual void AnimationFinishTrigger()
        {
            TriggerCalled = true;
        }
    }
}