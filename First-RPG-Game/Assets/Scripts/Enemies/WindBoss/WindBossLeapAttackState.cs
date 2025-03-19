using UnityEngine;

namespace Enemies.WindBoss
{
    public class WindBossLeapAttackState : EnemyState
    {
        private WindBoss _windBoss;
        private Transform _hammer;
        private float _countdown;
        private bool _attacked;
        private float _leapSpeed = 150f; // Adjust speed as needed
        private Vector3 _targetPosition;

        public WindBossLeapAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName,
            WindBoss windBoss) : base(enemyBase, stateMachine, animBoolName)
        {
            _windBoss = windBoss;
        }

        public override void Enter()
        {
            base.Enter();
            _hammer = Object.FindFirstObjectByType<WindBossHammerController>()?.transform;

            if (_hammer)
            {
                _targetPosition = GetHammerGroundPosition();
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

                if (Vector3.Distance(_windBoss.transform.position, _targetPosition) == 0) 
                {
                    _attacked = true;
                    _windBoss.lastTimeAttacked = Time.time;
                    StateMachine.ChangeState(_windBoss.BattleState);
                }
            }
        }

        public override void Exit()
        {
            _attacked = false;
            base.Exit();
        }

        private Vector3 GetHammerGroundPosition()
        {
            RaycastHit hit;
            
            if (Physics.Raycast(_hammer.position, Vector3.down, out hit, 10))
            {
                return new Vector3(_hammer.position.x, hit.point.y, _hammer.position.z);
            }

            return _hammer.position;
        }
    }
}
