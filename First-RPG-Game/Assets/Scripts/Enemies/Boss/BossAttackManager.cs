using UnityEngine;
using System.Collections;
using Skills;
using MainCharacter;

namespace Enemies.Boss
{
    public class BossAttackManager : MonoBehaviour
    {
        //public static BossAttackManager Instance { get; private set; }

        public EnemyBoss boss;

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

            if (boss.GetCurrentState() == boss.AttackState)
            {
                HandleAttackLogic();
            }

        }

        public void HandleAttackLogic()
        {
            
            float hpPercent = (float)boss.Stats.currentHp / boss.Stats.maxHp.ModifiedValue;
            //Debug.Log("hp:  " + boss.Stats.currentHp + "-----" + boss.Stats.maxHp.ModifiedValue + "======" + hpPercent*100);
            //Debug.Log("count: " + normalAttackCountSinceLastSkill);
            //Debug.Log("nomal: " + CanUseNormalAttack());*/
            if (Time.time >= boss.lastTimeAttacked)
            {
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
        }

        private void PhaseOne()
        {
            //Debug.Log("count: " + normalAttackCountSinceLastSkill);
            if (normalAttackCountSinceLastSkill >= 2 && CanUseSkill1())
            {
                normalAttackCountSinceLastSkill = 0;
                UseSkill1();
            }
            else if (CanUsePierceAttack())
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
            else if (CanUsePierceAttack())
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
            else if (CanUsePierceAttack())
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
            //Debug.Log("can slash");
            return Time.time >= lastSlashUpAttackTime + boss.SlashUpCooldown;
        }
        private bool CanUsePierceAttack()
        {
            //Debug.Log("pierce");
            return Time.time >= lastPierceAttackTime + boss.PierceCooldown;
        }
        private bool CanUseSkill1()
        {
            //Debug.Log("can skill1");
            return Time.time >= lastSkill1AttackTime + boss.Skill1Cooldown;
        }
        private bool CanUseSkill2()
        {
            return Time.time >= lastSkill2AttackTime + boss.Skill2Cooldown;
        }

        private bool CanUseUlti()
        {
            return canUseUlti && Time.time >= lastUltiAttackTime + boss.ultiCooldown;
        }
        private void UseSlashUp()
        {
            boss.PerformAttack("SlashUp");
            // Lấy thời gian của animation hiện tại
            boss.Stats.damage.SetDefaultValue(boss.SlashUpDamage);
            float animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            // Cập nhật lastSlashUpAttackTime thành thời gian kết thúc chiêu
            lastSlashUpAttackTime = Time.time + animationDuration;
            boss.lastTimeAttacked = Time.time + animationDuration;
            boss.attackCooldown = animationDuration;

        }
        private void UsePierce()
        {
            boss.PerformAttack("Pierce");
            boss.Stats.damage.SetDefaultValue(boss.PierceDamage);
            float animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            lastPierceAttackTime = Time.time + animationDuration;
            boss.lastTimeAttacked = Time.time;
            boss.attackCooldown = animationDuration;
        }

        private void UseSkill1()
        {
            boss.PerformAttack("Skill1");
            boss.Stats.damage.SetDefaultValue(boss.Skill1Damage);
            float animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            lastSkill1AttackTime = Time.time + animationDuration;
            boss.lastTimeAttacked = Time.time;
            boss.attackCooldown = animationDuration;
        }
        private void UseSkill2()
        {
            boss.PerformAttack("Skill2");
            boss.Stats.damage.SetDefaultValue(boss.Skill2Damage);
            float animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            lastSkill2AttackTime = Time.time + animationDuration;
            boss.lastTimeAttacked = Time.time;
            boss.attackCooldown = animationDuration;
        }

        private void UseUlti()
        {
            boss.PerformAttack("Ulti");
            boss.Stats.damage.SetDefaultValue(boss.ultiDamage);
            float animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            lastUltiAttackTime = Time.time + animationDuration;
            boss.lastTimeAttacked = Time.time;
            boss.attackCooldown = animationDuration;
        }
    }
}
