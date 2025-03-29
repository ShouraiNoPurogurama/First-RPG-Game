using MainCharacter;
using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossMeleeAttackState : EnemyState
    {
        private WindBoss _windBoss;
        private Transform _player;

        public WindBossMeleeAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, WindBoss windBoss) :
            base(enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }

        public override void Enter()
        {
            base.Enter();
            AttachCurrentPlayerIfNotExists();
        }

        public override void Update()
        {
            base.Update();
            
            if (!_windBoss.IsBusy)
            {
                FaceToPlayer();

                _windBoss.SetVelocity(_windBoss.FacingDir * _windBoss.defaultMoveSpeed * 2.5f, Rb.linearVelocity.y);
            }
            else
            {
                _windBoss.SetZeroVelocity();
            }

            if (TriggerCalled)
            {
                TriggerCalled = false;
                _windBoss.lastTimeAttacked = Time.time;
                StateMachine.ChangeState(_windBoss.BattleState);
            }
        }

        private void AttachCurrentPlayerIfNotExists()
        {
            _windBoss.SetZeroVelocity();
            if (!_player)
            {
                _player = PlayerManager.Instance.player.transform;
            }
        }

        private void FaceToPlayer()
        {
            if (_player.transform.position.x > _windBoss.transform.position.x && _windBoss.FacingDir == -1)
                _windBoss.Flip();
            else if (_player.transform.position.x < _windBoss.transform.position.x && _windBoss.FacingDir == 1)
                _windBoss.Flip();
        }
    }
}