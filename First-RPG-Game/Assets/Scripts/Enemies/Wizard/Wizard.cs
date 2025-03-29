using UnityEngine;

namespace Enemies.Wizard
{
    public class EnemyWizard : Enemy
    {
        public WizardIdleState IdleState { get; private set; }
        public WizardMoveState MoveState { get; private set; }
        public WizardBattleState BattleState { get; private set; }
        public WizardAttackState AttackState { get; private set; }
        public WizardStunnedState StunnedState { get; private set; }
        public WizardDeadState DeadState { get; private set; }

        [Header("Scaling Properties")]
        [SerializeField] private float sizeMultiplier = 1.0f;
        [SerializeField] private float damageMultiplier = 1.0f;
        [SerializeField] private float sizeIncreaseFactor = 0.1f;
        [SerializeField] private float damageIncreaseFactor = 0.2f;
        [SerializeField] private int baseDamage = 10;
        [SerializeField] private float maxSizeMultiplier = 3.0f;
        [SerializeField] private float maxDamageMultiplier = 5.0f;

        public int CurrentDamage => Mathf.RoundToInt(baseDamage * damageMultiplier);

        protected override void Awake()
        {
            base.Awake();

            IdleState = new WizardIdleState(this, StateMachine, "Idle", this);
            MoveState = new WizardMoveState(this, StateMachine, "Move", this);
            BattleState = new WizardBattleState(this, StateMachine, "Move", this);
            AttackState = new WizardAttackState(this, StateMachine, "Attack", this);
            StunnedState = new WizardStunnedState(this, StateMachine, "Stunned", this);
            DeadState = new WizardDeadState(this, StateMachine, "Idle", this);

            counterImage.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);
        }

        public override bool IsCanBeStunned(bool forceStun)
        {
            if (base.IsCanBeStunned(forceStun))
            {
                StateMachine.ChangeState(StunnedState);
                return true;
            }
            return false;
        }

        public override void Flip()
        {
            if (IsBusy)
                return;

            base.Flip();
            StartCoroutine("BusyFor", .3f);
        }

        public void OnTakeHit()
        {
            sizeMultiplier = Mathf.Min(sizeMultiplier + sizeIncreaseFactor, maxSizeMultiplier);
            damageMultiplier = Mathf.Min(damageMultiplier + damageIncreaseFactor, maxDamageMultiplier);

            transform.localScale = Vector3.one * sizeMultiplier;

            if (FX != null)
            {
                FX.Invoke("PlayHitEffect", 0);
            }
        }

        public override void Die()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 10);
            StateMachine.ChangeState(DeadState);
            base.Die();
        }
    }
}