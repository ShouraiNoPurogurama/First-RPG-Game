using UnityEngine;
using System.Collections;

namespace Enemies.Boss
{
    public class BossAttackManager : MonoBehaviour
    {
        private EnemyBoss boss;

        //private float lastAttackTime;
        //private int attackCount = 0;
        private float lastSlashUpAttackTime;
        private float lastPierceAttackTime;
        private float lastSkill1AttackTime;
        private float lastSkill2AttackTime;
        private float lastUltiAttackTime;


        private int normalAttackCountSinceLastSkill = 0;

        private bool canUseUlti = false;

        void Start()
        {
            boss = GetComponent<EnemyBoss>();
        }

        void Update()
        {
            HandleAttackLogic();
        }

        public void HandleAttackLogic()
        {
            
            float hpPercent = (float)boss.Stats.currentHp / boss.Stats.maxHp.ModifiedValue;
            //Debug.Log("hp:  " + boss.Stats.currentHp + "-----" + boss.Stats.maxHp.ModifiedValue + "======" + hpPercent*100);
            //Debug.Log("count: " + normalAttackCountSinceLastSkill);
            //Debug.Log("nomal: " + CanUseNormalAttack());*/
            if (hpPercent > 0.7f)
            {
                PhaseOne();
            }
            else if (hpPercent > 0.5f)
            {
                PhaseOne();
            }
            else if (hpPercent > 0.3f)
            {
                PhaseThree();
            }
            else
            {
                canUseUlti = true;
                PhaseFour();
            }
        }

        private void PhaseOne()
        {
            Debug.Log("count: " + normalAttackCountSinceLastSkill);
            if (normalAttackCountSinceLastSkill >= 2 && CanUseSkill1())
            {
                normalAttackCountSinceLastSkill = 0;
                UseSkill1();
            }
            else if (normalAttackCountSinceLastSkill >= 1 && CanUsePierceAttack())
            {
                normalAttackCountSinceLastSkill++;
                UsePierce();
            }
            else if (CanUseSlashUpAttack())
            {
                normalAttackCountSinceLastSkill++;
                UseSlashUp();
            }
        }
        private void PhaseThree()
        {
            if (normalAttackCountSinceLastSkill >= 3 && CanUseSkill2())
            {
                normalAttackCountSinceLastSkill = 0;
                UseSkill2();
            }
            else if (normalAttackCountSinceLastSkill >= 2 && CanUseSkill1())
            {
                normalAttackCountSinceLastSkill++;
                UseSkill1();
            }
            else if (normalAttackCountSinceLastSkill >= 1 && CanUsePierceAttack())
            {
                normalAttackCountSinceLastSkill++;
                UsePierce();
            }
            else if (CanUseSlashUpAttack())
            {
                normalAttackCountSinceLastSkill++;
                UseSlashUp();
            }
        }

        private void PhaseFour()
        {
            if (normalAttackCountSinceLastSkill >= 4 && CanUseUlti())
            {
                normalAttackCountSinceLastSkill = 0;
                UseUlti();
            }
            else if (normalAttackCountSinceLastSkill >= 3 && CanUseSkill2())
            {
                normalAttackCountSinceLastSkill++;
                UseSkill2();
            }
            else if (normalAttackCountSinceLastSkill >= 2 && CanUseSkill1())
            {
                normalAttackCountSinceLastSkill++;
                UseSkill1();
            }
            else if (normalAttackCountSinceLastSkill >= 1 && CanUsePierceAttack())
            {
                normalAttackCountSinceLastSkill++;
                UsePierce();
            }
            else if (CanUseSlashUpAttack())
            {
                normalAttackCountSinceLastSkill++;
                UseSlashUp();
            }
        }
        private bool CanUseSlashUpAttack()
        {
            return Time.time >= lastSlashUpAttackTime + boss.SlashUpCooldown;
        }
        private bool CanUsePierceAttack()
        {
            return Time.time >= lastPierceAttackTime + boss.PierceCooldown;
        }
        private bool CanUseSkill1()
        {
            return Time.time >= lastSkill1AttackTime + boss.Skill1Cooldown;
        }
        private bool CanUseSkill2()
        {
            return Time.time >= lastSkill2AttackTime + boss.Skill2Cooldown;
        }

        private bool CanUseUlti()
        {
            return canUseUlti &&
                   Time.time >= lastUltiAttackTime + boss.ultiCooldown;
        }
        private void UseSlashUp()
        {
            //Debug.Log("use slUp");
            //boss.Animator.SetTrigger(BossAnimationTrigger.NormalAttack);
            boss.PerformAttack("SlashUp");
            // Lấy thời gian của animation hiện tại
            boss.Stats.damage.SetDefaultValue(boss.SlashUpDamage);
            float animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            // Cập nhật lastSlashUpAttackTime thành thời gian kết thúc chiêu
            lastSlashUpAttackTime = Time.time + animationDuration;
            boss.lastTimeAttacked = Time.time + animationDuration;

        }
        private void UsePierce()
        {
            //boss.Animator.SetTrigger(BossAnimationTrigger.NormalAttack);
            boss.PerformAttack("Pierce");
            boss.Stats.damage.SetDefaultValue(boss.PierceDamage);
            float animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            lastPierceAttackTime = Time.time + animationDuration;
            boss.lastTimeAttacked = Time.time + animationDuration;
        }

        private void UseSkill1()
        {
            //boss.Animator.SetTrigger(BossAnimationTrigger.SkillAttack);
            boss.PerformAttack("Skill1");
            boss.Stats.damage.SetDefaultValue(boss.Skill1Damage);
            float animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            lastSkill1AttackTime = Time.time + animationDuration;
            boss.lastTimeAttacked = Time.time + animationDuration;
        }
        private void UseSkill2()
        {
            //boss.Animator.SetTrigger(BossAnimationTrigger.SkillAttack);
            boss.PerformAttack("Skill2");
            boss.Stats.damage.SetDefaultValue(boss.Skill2Damage);
            float animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            lastSkill2AttackTime = Time.time + animationDuration;
            boss.lastTimeAttacked = Time.time + animationDuration;
        }

        private void UseUlti()
        {
            //boss.Animator.SetTrigger(BossAnimationTrigger.UltiAttack);
            boss.PerformAttack("Ulti");
            boss.Stats.damage.SetDefaultValue(boss.ultiDamage);
            float animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            lastUltiAttackTime = Time.time + animationDuration;
            boss.lastTimeAttacked = Time.time + animationDuration;
        }
        //private void UseSlashUp()
        //{
        //    StartCoroutine(AttackCoroutine("SlashUp", boss.SlashUpDamage, lastSlashUpAttackTime, boss.SlashUpCooldown));
        //}

        //private void UsePierce()
        //{
        //    StartCoroutine(AttackCoroutine("Pierce", boss.PierceDamage, lastPierceAttackTime, boss.PierceCooldown));
        //}

        //private void UseSkill1()
        //{
        //    StartCoroutine(AttackCoroutine("Skill1", boss.Skill1Damage, lastSkill1AttackTime, boss.Skill1Cooldown));
        //}

        //private void UseSkill2()
        //{
        //    StartCoroutine(AttackCoroutine("Skill2", boss.Skill2Damage, lastSkill2AttackTime, boss.Skill2Cooldown));
        //}

        //private void UseUlti()
        //{
        //    StartCoroutine(AttackCoroutine("Ulti", boss.ultiDamage, lastUltiAttackTime, boss.ultiCooldown));
        //}
        //private IEnumerator AttackCoroutine(string attackType, int damage, float lastAttackTime, float cooldown)
        //{
        //    if (boss.isAttacking) yield break; // Nếu boss đang tấn công, không làm gì cả

        //    boss.isAttacking = true; // Đánh dấu boss đang tấn công
        //    boss.PerformAttack(attackType);
        //    boss.Stats.damage.SetDefaultValue(damage);

        //    // Chờ animation kết thúc
        //    yield return new WaitUntil(() => boss.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        //    // Cập nhật thời gian tấn công
        //    lastAttackTime = Time.time + cooldown;
        //    boss.lastTimeAttacked = Time.time + cooldown;

        //    boss.isAttacking = false; // Đánh dấu kết thúc tấn công
        //}

    }
}
