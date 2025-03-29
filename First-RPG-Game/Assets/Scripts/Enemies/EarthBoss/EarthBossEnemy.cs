using Enemies;
using Enemies.Boss;
using Skills;
using UnityEngine;

namespace Enemies.EarthBoss
{
    public class EarthBossEnemy : Enemy
    {

        [Header("CooldownSkill")]
        public float SlashUpCooldown = 2f;
        public float PierceCooldown = 2f; 
        public float Skill1Cooldown = 6f;
        public float Skill2Cooldown = 6f;
        public float ultiCooldown = 10f; 
        [Header("DamageSkill")]
        public int SlashUpDamage = 30; 
        public int PierceDamage = 50;
        public int Skill1Damage = 70;
        public int Skill2Damage = 80;
        public int ultiDamage = 150;

        [SerializeField] public Vector2 knockBackPlayer = new Vector2(5, 5);
        #region States
        public SkillManager SkillManager { get; private set; }
        public GameObject ThrownStone { get; private set; }
        #endregion
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            SkillManager = SkillManager.Instance;
            //attackManager = BossAttackManager.Instance;
        }


        protected override void Update()
        {
            base.Update();
        }

        public void PerformAttack(string attackType)
        {
        }


        public override void Flip()
        {
            if (IsBusy)
                return;

            base.Flip();

            StartCoroutine("BusyFor", .3f);
        }

        public override void Die()
        {
            base.Die();
        }
        public EnemyState GetCurrentState()
        {
            return StateMachine.CurrentState;
        }
    }

}
