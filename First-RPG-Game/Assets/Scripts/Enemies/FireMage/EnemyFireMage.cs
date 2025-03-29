using Audio;
using UnityEngine;

namespace Enemies.FireMage
{
    public class EnemyFireMage : Enemy
    {
        public GameObject MeteoritePrefab;
        #region States
        public FireMageIdleState IdleState { get; private set; }
        public FireMageMoveState MoveState { get; private set; }
        public FireMageBattleState BattleState { get; private set; }
        public FireMageAttackState AttackState { get; private set; }
        public FireMageDeadState DeadState { get; private set; }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            IdleState = new FireMageIdleState(this, StateMachine, "Idle", this);
            MoveState = new FireMageMoveState(this, StateMachine, "Move", this);
            BattleState = new FireMageBattleState(this, StateMachine, "Move", this);
            AttackState = new FireMageAttackState(this, StateMachine, "Attack", this);
            DeadState = new FireMageDeadState(this, StateMachine, "Dead", this);
        }

        protected override void Start()
        {
            base.Start();

            StateMachine.Initialize(IdleState);
        }


        protected override void Update()
        {
            base.Update();
        }

        public override void Flip()
        {
            //Debug.Log("IsBusy: " + IsBusy);

            if (IsBusy)
                return;

            base.Flip();

            StartCoroutine("BusyFor", .3f);
        }

        public override void Die()
        {
            StateMachine.ChangeState(DeadState);
            base.Die();
        }
        public void SpawnFireballs()
        {
            SoundManager.PlaySFX("FireMage", 3, true);
            // random ball fall
            for (int i = 0; i < 5; i++)
            {
                Vector3 randomPos = transform.position + new Vector3(Random.Range(-20f, 20f), 20f, 0);
                Instantiate(MeteoritePrefab, randomPos, Quaternion.identity);
                randomPos = transform.position + new Vector3(Random.Range(-20f, 20f), 18f, 0);
                Instantiate(MeteoritePrefab, randomPos, Quaternion.identity);
                randomPos = transform.position + new Vector3(Random.Range(-20f, 20f), 19f, 0);
                Instantiate(MeteoritePrefab, randomPos, Quaternion.identity);
            }
        }


    }
}

