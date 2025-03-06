using UnityEngine;

namespace Enemies
{
    public class EnemyState
    {
        protected readonly EnemyStateMachine StateMachine;
        private readonly Enemy _enemyBase;
        protected Rigidbody2D Rb;

        protected bool TriggerCalled;
        private readonly string _animBoolName;
        protected float StateTimer;

        protected EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName)
        {
            StateMachine = stateMachine;
            _enemyBase = enemyBase;
            _animBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            TriggerCalled = false;
            Rb = _enemyBase.Rb;
            _enemyBase.Animator.SetBool(_animBoolName, true);
            
            _enemyBase.AssignLastAnimBoolName(_animBoolName); //Anim bool name will be used for dead state
        }

        public virtual void Update()
        {
            StateTimer -= Time.deltaTime;
        }

        public virtual void Exit()
        {
            _enemyBase.Animator.SetBool(_animBoolName, false);
        }

        public virtual void AnimationFinishTrigger()
        {
            TriggerCalled = true;
        }
    }
}