using UnityEngine;
using System.Collections;

namespace Enemies.WindBoss
{
    public class WindBossLeapAttackState : EnemyState
    {
        private WindBoss _windBoss;
        private WindBossHammerController _hammer;
        private float _countdown;
        private bool _attacked;
        private float _leapSpeed = 150f;
        private Vector3 _targetPosition;
        private Vector3 _previousPosition;

        public WindBossLeapAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName,
            WindBoss windBoss) : base(enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }

        public override void Enter()
        {
            base.Enter();
            _hammer = Object.FindFirstObjectByType<WindBossHammerController>();

            if (_hammer)
            {
                _targetPosition = GetHammerGroundPosition();
            }
            
            FaceToPlayer();
        }

        private void FaceToPlayer()
        {
            if(_targetPosition.x < _windBoss.transform.position.x && _windBoss.FacingDir == 1)
            {
                _windBoss.Flip();
            }
            else if(_targetPosition.x > _windBoss.transform.position.x && _windBoss.FacingDir == -1)
            {
                _windBoss.Flip();
            }
        }

        public override void Update()
        {
            base.Update();

            _countdown -= Time.deltaTime;

            if (!_attacked && _hammer && _countdown <= 0)
            {
                _windBoss.transform.position = Vector3.MoveTowards(
                    _windBoss.transform.position,
                    _targetPosition,
                    _leapSpeed * Time.deltaTime
                );

                if (HasLanded() || TriggerCalled)
                {
                    _attacked = true;
                    _windBoss.lastTimeAttacked = Time.time;

                    _windBoss.StartCoroutine(WaitAndChangeState(1f));
                }

                _previousPosition = _windBoss.transform.position;
            }
        }

        public override void Exit()
        {
            TriggerCalled = false;
            _attacked = false;
            base.Exit();
        }

        private Vector3 GetHammerGroundPosition()
        {
            var hammerTransform = _hammer.transform;
            
            float xOffset = _windBoss.transform.position.x < hammerTransform.position.x ? -1f : 1f;
            
            var targetPosition = new Vector3(hammerTransform.position.x + xOffset, hammerTransform.position.y, 0f);
            return targetPosition;
        }

        private bool HasLanded()
        {
            return Mathf.Approximately(_windBoss.transform.position.y, _previousPosition.y);
        }

        private IEnumerator WaitAndChangeState(float delay)
        {
            _hammer.SelfDestroy();
            yield return new WaitForSeconds(delay);
            StateMachine.ChangeState(_windBoss.BattleState);
        }
    }
}