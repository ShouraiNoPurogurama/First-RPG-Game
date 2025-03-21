using MainCharacter;
using UnityEngine;

namespace Enemies.Archer
{
    public class ArcherRunState : ArcherGroundedState
    {
        private Transform _player;

        public ArcherRunState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher archer) : base(
            enemyBase, stateMachine, animBoolName, archer)
        {
        }

        public override void Enter()
        {
            base.Enter();

            AttachCurrentPlayerIfNotExists();
            StateTimer = Archer.runDuration;
            
            FaceAwayFromPlayer();
        }

        public override void Update()
        {
            base.Update();

            if (Archer.IsWallDetected() || !Archer.IsGroundDetected())
            {
                Archer.Flip();
            }

            float xVelocity = Archer.runSpeed * Archer.FacingDir;

            Archer.SetVelocity(xVelocity, 0);

            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(Archer.BattleState);
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
            if (_player.transform.position.x > Archer.transform.position.x && Archer.FacingDir == 1)
                Archer.Flip();
            else if (_player.transform.position.x < Archer.transform.position.x && Archer.FacingDir == -1)
                Archer.Flip();
        }
    }
}