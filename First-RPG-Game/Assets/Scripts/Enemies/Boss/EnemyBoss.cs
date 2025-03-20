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

        #region States
        public bool isAttacking = false;

        public BossAttackManager attackManager;
        public BossIdleState IdleState { get; private set; }
        public BossMoveState MoveState { get; private set; }
        public BossBattleState BattleState { get; private set; }
        public BossAttackState AttackState { get; private set; }
        public BossDeadState DeadState { get; private set; }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            IdleState = new BossIdleState(this, StateMachine, "Idle", this);
            MoveState = new BossMoveState(this, StateMachine, "Move", this);
            BattleState = new BossBattleState(this, StateMachine, "Move", this);
            AttackState = new BossAttackState(this, StateMachine, "SlashUp", this);
            DeadState = new BossDeadState(this, StateMachine, "Dead", this);
            attackManager = GetComponent<BossAttackManager>();
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

        public void PerformAttack(string attackType)
        {
            Debug.Log("Boss thực hiện chiêu: " + attackType);

            switch (attackType)
            {
                case "SlashUp":
                    AttackState = new BossAttackState(this, StateMachine, "SlashUp", this);
                    //animator.SetTrigger("SlashUp");
                    break;
                case "Pierce":
                    AttackState = new BossAttackState(this, StateMachine, "Pierce", this);
                    //animator.SetTrigger("Pierce");
                    break;
                case "Skill1":
                    AttackState = new BossAttackState(this, StateMachine, "Skill1", this);
                    //animator.SetTrigger("Skill1");
                    break;
                case "Skill2":
                    AttackState = new BossAttackState(this, StateMachine, "Skill2", this);
                    //animator.SetTrigger("Skill1");
                    break;
                case "Ulti":
                    AttackState = new BossAttackState(this, StateMachine, "Ulti", this);
                    //animator.SetTrigger("Ulti");
                    break;
            }
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

    }
}

