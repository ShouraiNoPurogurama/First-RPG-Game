using UnityEngine;

namespace Enemies.FireMiniMage
{
    public class EnemyFireMiniMage : Enemy
    {
        public float safeDistance;
        public float throwballDistance;
        public GameObject fireballPrefab;
        [SerializeField] public float fireballSpeed;
        [SerializeField] public float fireballDamage;
        #region States
        public FireMiniMageIdleState IdleState { get; private set; }
        public FireMiniMageMoveState MoveState { get; private set; }
        public FireMiniMageBattleState BattleState { get; private set; }
        public FireMiniMageAttackState AttackState { get; private set; }
        public FireMiniMageDeadState DeadState { get; private set; }
        public FireMiniMageAttackThrowFireballState ThrowAttackState { get; private set; }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            IdleState = new FireMiniMageIdleState(this, StateMachine, "Idle", this);
            MoveState = new FireMiniMageMoveState(this, StateMachine, "Move", this);
            BattleState = new FireMiniMageBattleState(this, StateMachine, "Move", this);
            AttackState = new FireMiniMageAttackState(this, StateMachine, "Attack1", this);
            DeadState = new FireMiniMageDeadState(this, StateMachine, "Dead", this);
            ThrowAttackState = new FireMiniMageAttackThrowFireballState(this, StateMachine, "Attack2", this);
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
        /*public override void AnimationSpecialAttackTrigger()
        {
            base.AnimationSpecialAttackTrigger();

            Vector3 spawnPosition = attackCheck.position - new Vector3(1f * FacingDir, 0, 0);
            // Tạo fire ball prefab tại vị trí attackCheck
            GameObject newfireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);

            // Thiết lập hướng bay cho fireball
            newfireball.GetComponent<FireballController>().SetupFireball(fireballSpeed * FacingDir, Stats);
        }*/
    }
}

