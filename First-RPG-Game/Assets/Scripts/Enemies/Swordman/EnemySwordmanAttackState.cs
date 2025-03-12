//using Enemies.Swordman;
//using UnityEngine;

//namespace Enemies.Swordman
//{
//    public class EnemySwordmanAttackState : EnemyState
//    {
//        private EnemySwordman _enemySwordman;

//        public EnemySwordmanAttackState(EnemySwordman enemy, EnemyStateMachine stateMachine, string animBoolName)
//            : base(enemy, stateMachine, animBoolName)
//        {
//            _enemySwordman = enemy;
//        }

//        public override void Enter()
//        {
//            base.Enter();
//            _enemySwordman.Animator.SetTrigger("isAttacking");
//        }

//        public override void AnimationFinishTrigger()
//        {
//            base.AnimationFinishTrigger();
//            StateMachine.ChangeState(new EnemySwordmanMoveState(_enemySwordman, StateMachine, "isChasing"));
//        }
//    }
//}
