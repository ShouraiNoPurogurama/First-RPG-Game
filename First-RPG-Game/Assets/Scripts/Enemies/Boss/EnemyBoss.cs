using UnityEngine;

namespace Enemies.Boss
{
    public class EnemyBoss : Enemy
    {
        [Header("CooldownSkill")]
        public float SlashUpCooldown = 2f; // Hồi nhanh
        public float PierceCooldown = 2f; // Hồi nhanh
        public float Skill1Cooldown = 6f;
        public float Skill2Cooldown = 6f;// Hồi gấp 3 lần chiêu thường
        public float ultiCooldown = 10f; // Hồi gấp 5 lần chiêu thường
        [Header("DamageSkill")]
        public int SlashUpDamage = 30; // Hồi nhanh
        public int PierceDamage = 50; // Hồi nhanh
        public int Skill1Damage = 70;
        public int Skill2Damage = 80;// Hồi gấp 3 lần chiêu thường
        public int ultiDamage = 150; // Hồi gấp 5 lần chiêu thường

        [SerializeField] public Vector2 knockBackPlayer = new Vector2(5, 5);
        #region States

        public BossIdleState IdleState { get; private set; }
        public BossMoveState MoveState { get; private set; }
        public BossBattleState BattleState { get; private set; }
        public BossAttackSlashUpState AttackSlashUpState { get; private set; }
        public BossAttackPierceState AttackPierceState { get; private set; }
        public BossAttackSkill1State AttackSkill1State { get; private set; }
        public BossAttackSkill2State AttackSkill2State { get; private set; }
        public BossAttackUltiState AttackUltiState { get; private set; }
        public BossDeadState DeadState { get; private set; }
        public BossAttackManager attackManager { get; private set; }
        #endregion
        protected override void Awake()
        {
            base.Awake();
            IdleState = new BossIdleState(this, StateMachine, "Idle", this);
            MoveState = new BossMoveState(this, StateMachine, "Move", this);
            BattleState = new BossBattleState(this, StateMachine, "Move", this);
            AttackSlashUpState = new BossAttackSlashUpState(this, StateMachine, "SlashUp", this);
            AttackPierceState = new BossAttackPierceState(this, StateMachine, "Pierce", this);
            AttackSkill1State = new BossAttackSkill1State(this, StateMachine, "Skill1", this);
            AttackSkill2State = new BossAttackSkill2State(this, StateMachine, "Skill2", this);
            AttackUltiState = new BossAttackUltiState(this, StateMachine, "Ulti", this);
            DeadState = new BossDeadState(this, StateMachine, "Dead", this);
            attackManager = GetComponent<BossAttackManager>();
        }

        protected override void Start()
        {
            base.Start();

            StateMachine.Initialize(IdleState);
            //attackManager = BossAttackManager.Instance;
        }


        protected override void Update()
        {
            base.Update();
        }

        public void PerformAttack(string attackType)
        {
            //Debug.Log("Boss thực hiện chiêu: " + attackType);

            switch (attackType)
            {
                case "SlashUp":
                    StateMachine.ChangeState(AttackSlashUpState);
                    break;
                case "Pierce":
                    StateMachine.ChangeState(AttackPierceState);
                    break;
                case "Skill1":
                    StateMachine.ChangeState(AttackSkill1State);
                    break;
                case "Skill2":
                    StateMachine.ChangeState(AttackSkill2State);
                    break;
                case "Ulti":
                    StateMachine.ChangeState(AttackUltiState);
                    break;
            }
            return;
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
        public EnemyState GetCurrentState()
        {
            return StateMachine.CurrentState;
        }
    }
}

