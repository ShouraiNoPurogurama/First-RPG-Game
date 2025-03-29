using System.Collections;
using UnityEngine;

namespace Enemies.DarkBoss
{
    public class DarkBossCastState : EnemyState
    {
        private DarkBoss _darkBoss;
        private Vector2 _targetPosition; 
        private float _moveSpeed = 3f;
        private float defaultGravityScale;
        private Coroutine shootCoroutine;

        public DarkBossCastState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, DarkBoss darkBoss)
            : base(enemyBase, stateMachine, animBoolName)
        {
            _darkBoss = darkBoss;
            _targetPosition = new Vector2(0, 0); 
        }

        public override void Enter()
        {
            base.Enter();
            defaultGravityScale = _darkBoss.Rb.gravityScale;
            _darkBoss.Rb.gravityScale = 0;
            StateTimer = 5f; 
            MoveToCenter();
            shootCoroutine = _darkBoss.StartCoroutine(ShootArrowsPeriodically());
        }

        public override void Update()
        {
            base.Update();

            // Move towards the target position
            _darkBoss.transform.position = Vector2.MoveTowards(_darkBoss.transform.position, _targetPosition, _moveSpeed * Time.deltaTime);


            // Transition to the MoveState if the timer runs out
            if (StateTimer <= 0)
            {
                StateMachine.ChangeState(_darkBoss.MoveState);
            }
        }

        private void MoveToCenter()
        {
            // Set the target position to the center of the arena
            _targetPosition = _darkBoss.arena.bounds.center;
        }

        private IEnumerator ShootArrowsPeriodically()
        {
            while (StateTimer > 0)
            {
                _darkBoss.ShootArrows();
                yield return new WaitForSeconds(0.5f);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _darkBoss.Rb.gravityScale = defaultGravityScale;

            if (shootCoroutine != null)
            {
                _darkBoss.StopCoroutine(shootCoroutine);
                shootCoroutine = null;
            }
        }
    }


}
