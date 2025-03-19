using Enemies.Frogger;
using UnityEngine;

namespace Enemies.Frogger
{
    public class FroggerGroundedState : EnemyState
    {
        protected readonly EnemyFrogger Frogger;

        protected Transform Player;

        protected FroggerGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyFrogger frogger) :
            base(enemyBase, stateMachine, animBoolName)
        {
            Frogger = frogger;
        }

        public override void Enter()
        {
            base.Enter();

            Player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();
            
            if (StateTimer <= 0 && (Frogger.IsPlayerDetected() || Vector2.Distance(Frogger.transform.position, Player.position) < Frogger.attackDistance + 5))
            {
                StateMachine.ChangeState(Frogger.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}