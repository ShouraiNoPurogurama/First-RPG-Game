using MainCharacter;
using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossDashState : EnemyState
    {
        private Transform _player;
        private WindBoss _windBoss;
        private float dashDuration = 0.25f;
        private float dashSpeed = 15f;
        private int dashDir;

        public WindBossDashState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) : base(
            enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }

        public override void Enter()
        {
            base.Enter();

            AttachCurrentPlayerIfNotExists();
            StateTimer = dashDuration;

            FaceToPlayer();
        }

        public override void Update()
        {
            base.Update();

            dashDir = Rb.position.x < _player.position.x ? 1 : -1;

            if (Mathf.Abs(Rb.transform.position.x - _player.position.x) >= 1f)
            {
                float xVelocity = dashSpeed * dashDir;
                _windBoss.SetVelocity(xVelocity, Rb.linearVelocity.y);
            }

            if (StateTimer <= 0 && _windBoss.IsPlayerDetected().distance != 0 &&
                _windBoss.IsPlayerDetected().distance < _windBoss.attackDistance)
            {
                StateMachine.ChangeState(_windBoss.MeleeAttackState);
            }
        }

        public override void Exit()
        {
            _windBoss.lastTimeDashed = Time.time;
            _windBoss.SetZeroVelocity();
            base.Exit();
        }

        private void AttachCurrentPlayerIfNotExists()
        {
            if (!_player)
            {
                _player = PlayerManager.Instance.player.transform;
            }
        }

        private void FaceToPlayer()
        {
            if (_player.transform.position.x < _windBoss.transform.position.x && _windBoss.FacingDir == 1)
            {
                _windBoss.Flip();
            }
            else if (_player.transform.position.x > _windBoss.transform.position.x && _windBoss.FacingDir == -1)
            {
                _windBoss.Flip();
            }
        }
    }
}