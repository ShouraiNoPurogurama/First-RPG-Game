using MainCharacter;
using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossRunState : WindBossGroundedState
    {
        private Transform _player;

        public WindBossRunState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) : base(
            enemyBase, stateMachine, animBoolName, windBoss)
        {
        }

        public override void Enter()
        {
            base.Enter();

            AttachCurrentPlayerIfNotExists();
            StateTimer = WindBoss.runDuration;
            
            FaceAwayFromPlayer();
        }

        public override void Update()
        {
            base.Update();

            if (WindBoss.IsWallDetected() || !WindBoss.IsGroundDetected())
            {
                WindBoss.Flip();
            }

            float xVelocity = WindBoss.runSpeed * WindBoss.FacingDir;

            WindBoss.SetVelocity(xVelocity, 0);

            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(WindBoss.BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void AttachCurrentPlayerIfNotExists()
        {
            if (!_player)
            {
                _player = PlayerManager.Instance.player.transform;
            }
        }
        
        private void FaceAwayFromPlayer()
        {
            if (_player.transform.position.x > WindBoss.transform.position.x && WindBoss.FacingDir == 1)
                WindBoss.Flip();
            else if (_player.transform.position.x < WindBoss.transform.position.x && WindBoss.FacingDir == -1)
                WindBoss.Flip();
        }
    }
}