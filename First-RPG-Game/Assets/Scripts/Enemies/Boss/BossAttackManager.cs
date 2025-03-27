using UnityEngine;
using System.Collections;
using Skills;
using MainCharacter;
using Stats;

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
        private float animationDuration = 0;


        private int normalAttackCountSinceLastSkill = 0;

        private bool canUseUlti = false;

        void Start()
        {
            boss = GetComponent<EnemyBoss>();
        }

        void Update()
        {

        }

        public void HandleAttackLogic()
        {
            if (IsPerformingAttack()) return;

            float hpPercent = (float)boss.Stats.currentHp / boss.Stats.maxHp.ModifiedValue;
            //Debug.Log("hp:  " + boss.Stats.currentHp + "-----" + boss.Stats.maxHp.ModifiedValue + "======" + hpPercent*100);
            //Debug.Log("count: " + normalAttackCountSinceLastSkill);
            //Debug.Log("nomal: " + CanUseNormalAttack());*/
            if (Time.time >= boss.lastTimeAttacked)
            {
                if (hpPercent > 0.7f)
                {
                    //Debug.Log(boss.knockBackPlayer);
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
                UseSkill1();
                normalAttackCountSinceLastSkill = 0;
            }
            else if (CanUsePierceAttack() && normalAttackCountSinceLastSkill == 1)
            {
                UsePierce();
                normalAttackCountSinceLastSkill++;
            }
            else if (CanUseSlashUpAttack())
            {
                UseSlashUp();
                normalAttackCountSinceLastSkill++;
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
            else if (CanUsePierceAttack() && normalAttackCountSinceLastSkill == 1)
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
            else if (CanUsePierceAttack() && normalAttackCountSinceLastSkill == 1)
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
            return Time.time >= lastSlashUpAttackTime + boss.SlashUpCooldown ;
        }
        private bool CanUsePierceAttack()
        {
            //Debug.Log("pierce");
            return Time.time >= lastPierceAttackTime + boss.PierceCooldown ;
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
            //animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            boss.knockBackPlayer = new Vector2(1,15);
            //Debug.Log(boss.knockBackPlayer);
            boss.PerformAttack("SlashUp");
            boss.Stats.damage.SetDefaultValue(boss.SlashUpDamage);
            lastSlashUpAttackTime = Time.time + animationDuration;
            //boss.lastTimeAttacked = Time.time;
            //boss.attackCooldown = animationDuration;

        }
        private void UsePierce()
        {
            boss.knockBackPlayer = new Vector2(1, 5);
            //Debug.Log(boss.knockBackPlayer);
            //animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            boss.PerformAttack("Pierce");
            boss.Stats.damage.SetDefaultValue(boss.PierceDamage);
            lastPierceAttackTime = Time.time + animationDuration;
            //boss.lastTimeAttacked = Time.time;
            //boss.attackCooldown = animationDuration;
        }

        private void UseSkill1()
        {
            boss.knockBackPlayer = new Vector2(1, 15);
            //Debug.Log(boss.knockBackPlayer);
            //animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            boss.PerformAttack("Skill1");
            boss.Stats.damage.SetDefaultValue(boss.Skill1Damage);
            lastSkill1AttackTime = Time.time + animationDuration;
            //boss.lastTimeAttacked = Time.time;
            //boss.attackCooldown = animationDuration;

        }
        private void UseSkill2()
        {
            boss.knockBackPlayer = new Vector2(1, 15);
            boss.PerformAttack("Skill2");
            boss.Stats.damage.SetDefaultValue(boss.Skill2Damage);
            //animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            lastSkill2AttackTime = Time.time + animationDuration;
            //boss.lastTimeAttacked = Time.time;
            //boss.attackCooldown = animationDuration;
        }

        private void UseUlti()
        {
            boss.knockBackPlayer = new Vector2(1, 15);
            boss.PerformAttack("Ulti");
            boss.Stats.damage.SetDefaultValue(boss.ultiDamage);
            //animationDuration = boss.Animator.GetCurrentAnimatorStateInfo(0).length;
            lastUltiAttackTime = Time.time + animationDuration;
            //boss.lastTimeAttacked = Time.time;
            //boss.attackCooldown = animationDuration;
        }
        private bool IsPerformingAttack()
        {
            return boss.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
        }
    }
}
