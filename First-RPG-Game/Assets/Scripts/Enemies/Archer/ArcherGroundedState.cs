using UnityEngine;

namespace Enemies.Archer
{
    public class ArcherGroundedState : EnemyState
    {
        protected readonly EnemyArcher Archer;

        protected Transform Player;

        protected ArcherGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher archer) :
            base(enemyBase, stateMachine, animBoolName)
        {
            Archer = archer;
        }

        public override void Enter()
        {
            base.Enter();

            Player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();
            
            if (StateTimer <= 0 && (Archer.IsPlayerDetected() || Vector2.Distance(Archer.transform.position, Player.position) < ((Enemy)Archer).attackDistance + 5))
            {
                StateMachine.ChangeState(Archer.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}