using UnityEngine;

namespace Enemies.Orc
{
    public class OrcBattleState : EnemyState
    {
        private Orc Orc;
        private Transform Player;

        private int moveDir;
        public OrcBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Orc orc) : base(enemyBase, stateMachine, animBoolName)
        {
            this.Orc = orc;
        }

        public override void Enter()
        {
            base.Enter();
            Player = GameObject.Find("Player").transform;
        }

        public override void Update()
        {
            base.Update();
            moveDir = Player.position.x > Orc.transform.position.x ? 1 : -1;

            Orc.SetVelocity(Orc.moveSpeed * moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
