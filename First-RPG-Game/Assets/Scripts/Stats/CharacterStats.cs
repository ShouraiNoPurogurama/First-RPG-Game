using System;
using Enemies;
using MainCharacter;
using UI;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

namespace Stats
{
    public class CharacterStats : MonoBehaviour
    {
        private EntityFX _fx;

        [Header("Major stats")]
        public Stat strength; // = 1 dmg and 1% crit chance

        public Stat agility; // = 1% evasion and 1% crit chance
        public Stat intelligence; // = 1 magic dmg and 3% magic resistance
        public Stat vitality; //= 5 health

        [Header("Defensive stats")]
        public Stat maxHp;

        public Stat armor;
        public Stat evasion;
        public Stat magicResistance;

        [Header("Offensive stats")]
        public Stat damage;

        public Stat critChance;
        public Stat critPower;

        [Header("Magic stats")]
        public Stat fireDamage;

        public Stat iceDamage;
        public Stat lightingDamage;

        public bool isIgnited; //does damage over time
        public bool isChilled; // reduce armor by 30%
        public bool isShocked; // reduce attack accuracy by 20%

        [SerializeField] protected float ailmentDuration = 4;
        private float _igniteTimer;
        private float _chilledTimer;
        private float _shockedTimer;

        private float _igniteDamageCooldown = .5f;
        private float _igniteDamageTimer = .5f;
        private int _igniteDamage;
        private int _shockDamage;

        [SerializeField] private GameObject ShockEffectPrefab;

        [SerializeField] public int currentHp;

        public Action OnHealthChanged;

        public CharacterStats(Stat maxHp)
        {
            this.maxHp = maxHp;
        }

        protected virtual void Start()
        {
            _fx = GetComponent<EntityFX>();

            critPower.SetDefaultValue(150);
            currentHp = GetMaxHealthValue();

            damage.AddModifier(4);
        }

        protected virtual void Update()
        {
            _igniteTimer -= Time.deltaTime;
            _chilledTimer -= Time.deltaTime;
            _shockedTimer -= Time.deltaTime;


            _igniteDamageTimer -= Time.deltaTime;

            if (_igniteTimer <= 0)
            {
                isIgnited = false;
            }

            if (_chilledTimer <= 0)
            {
                isChilled = false;
            }

            if (_shockedTimer <= 0)
            {
                isShocked = false;
            }

            if (_igniteDamageTimer <= 0 && isIgnited)
            {
                DecreaseHealthBy(_igniteDamage, Color.red);

                if (currentHp < 0)
                {
                    Die();
                }

                _igniteDamageTimer = .3f;
            }
        }

        public void SetupIgniteDamage(int dmg) => _igniteDamage = dmg;
        public void SetupShockDamage(int dmg) => _shockDamage = dmg;

        public virtual void DoDamage(CharacterStats targetStats)
        {
            if (TargetCanDodgeAttack(targetStats))
            {
                return;
            }

            int totalDamage = damage.ModifiedValue + strength.ModifiedValue;

            if (CanCrit())
            {
                totalDamage = CalculateCriticalDamage(totalDamage);
            }

            totalDamage = DecreaseDamageByArmor(targetStats, totalDamage);

            //If equipments have ailment effects then do magical damage
            DoMagicalDamage(targetStats);

            //targetStats.TakeDamage(totalDamage);
        }

        public virtual void DoMagicalDamage(CharacterStats targetStats)
        {
            int fireDamageVal = fireDamage.ModifiedValue;
            int iceDamageVal = iceDamage.ModifiedValue;
            int lightingDamageVal = lightingDamage.ModifiedValue;

            int totalMagicalDamage = fireDamageVal + iceDamageVal + lightingDamageVal + intelligence.ModifiedValue;

            totalMagicalDamage = DecreaseDamageByResistance(targetStats, totalMagicalDamage);

            if (Mathf.Max(fireDamageVal, iceDamageVal, lightingDamageVal) <= 0)
            {
                return;
            }

            bool canApplyIgnite = fireDamageVal > iceDamageVal && fireDamageVal > lightingDamageVal;
            bool canApplyChill = iceDamageVal > fireDamageVal && iceDamageVal > lightingDamageVal;
            bool canApplyShock = lightingDamageVal > fireDamageVal && lightingDamageVal > iceDamageVal;

            Color magicDmgColor = Color.magenta;

            targetStats.TakeDamage(totalMagicalDamage, magicDmgColor);


            while (!canApplyIgnite && !canApplyChill && !canApplyShock)
            {
                if (Random.value < .5f && fireDamageVal > 0)
                {
                    canApplyIgnite = true;
                    targetStats.ApplyAilments(true, false, false);
                    return;
                }

                if (Random.value < .5f && iceDamageVal > 0)
                {
                    canApplyChill = true;
                    targetStats.ApplyAilments(false, true, false);
                    return;
                }

                if (Random.value < .5f && lightingDamageVal > 0)
                {
                    canApplyShock = true;
                    targetStats.ApplyAilments(false, false, true);
                    return;
                }
            }

            if (canApplyIgnite)
            {
                targetStats.SetupIgniteDamage(Mathf.RoundToInt(fireDamageVal * .4f));
            }

            if (canApplyShock)
            {
                targetStats.SetupShockDamage(Mathf.RoundToInt(lightingDamageVal * .4f));
            }

            targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        }

        private static int DecreaseDamageByResistance(CharacterStats targetStats, int totalMagicalDamage)
        {
            totalMagicalDamage -= targetStats.magicResistance.ModifiedValue + targetStats.intelligence.ModifiedValue * 3;

            totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
            return totalMagicalDamage;
        }

        public void ApplyAilments(bool ignite, bool chill, bool shock)
        {
            bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
            bool canApplyChill = !isIgnited && !isChilled && !isShocked;
            bool canApplyShock = !isIgnited && !isChilled;

            if (isIgnited || isChilled)
            {
                return;
            }

            if (ignite && canApplyIgnite)
            {
                isIgnited = true;
                _igniteTimer = ailmentDuration;

                _fx.IgniteFxFor(ailmentDuration);
            }

            if (chill && canApplyChill)
            {
                isChilled = true;
                _chilledTimer = ailmentDuration;

                var slowPercent = 0.3f;
                //Entity is the game object that being takes dmg
                GetComponent<Entity>().SlowEntityBy(slowPercent, ailmentDuration);

                _fx.ChillFxFor(ailmentDuration);
            }

            if (shock && canApplyShock)
            {
                if (!isShocked)
                {
                    ApplyShock(shock);
                }
                else
                {
                    if (GetComponent<Player>())
                    {
                        return;
                    }

                    HitNearestTargetWithShockStrike();
                }
            }

            isChilled = chill;
            isShocked = shock;
        }

        public void ApplyShock(bool shock)
        {
            if (isShocked) return;

            isShocked = shock;
            _shockedTimer = ailmentDuration;
            _fx.ShockFxFor(ailmentDuration);
        }

        private void HitNearestTargetWithShockStrike()
        {
            //Find the closest target among the enemies
            //Instantiate thunder strike
            var closestEnemy = FindClosestEnemy();

            if (closestEnemy)
            {
                GameObject newShockStrike = Instantiate(ShockEffectPrefab, transform.position, Quaternion.identity);

                newShockStrike.GetComponent<ShockStrikeController>()
                    .SetUp(_shockDamage, closestEnemy.GetComponent<CharacterStats>());
            }
        }

        private Transform FindClosestEnemy()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

            float closestDistance = math.INFINITY;
            Transform closestEnemy = null;

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() is not null && Vector2.Distance(transform.position, hit.transform.position) > 1)
                {
                    float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        closestEnemy = hit.transform;
                    }
                }

                if (!closestEnemy)
                {
                    closestEnemy = transform;
                }
            }

            return closestEnemy;
        }

        private static int DecreaseDamageByArmor(CharacterStats targetStats, int totalDamage)
        {
            if (targetStats.isChilled)
            {
                totalDamage -= Mathf.RoundToInt(targetStats.armor.ModifiedValue * .7f);
            }
            else
            {
                totalDamage -= targetStats.armor.ModifiedValue;
            }

            totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);

            return totalDamage;
        }

        private bool TargetCanDodgeAttack(CharacterStats targetStats)
        {
            int totalEvasion = targetStats.evasion.ModifiedValue + targetStats.agility.ModifiedValue;

            if (isShocked)
            {
                totalEvasion += 20;
            }

            if (Random.Range(0, 100) < totalEvasion)
            {
                return true;
            }

            return false;
        }

        public virtual void TakeDamage(int dmg, Color dmgColor = default)
        {
            GetComponent<Entity>().DamageImpact();

            _fx.Flash();

            if (dmgColor == default)
            {
                dmgColor = Color.white;
            }

            if (dmg > 0)
                _fx.CreatePopupText(dmg.ToString(), dmgColor);

            currentHp -= dmg;

            OnHealthChanged?.Invoke();

            if (currentHp <= 0)
            {
                Die();
            }
        }

        protected virtual void DecreaseHealthBy(int dmg, Color dmgColor)
        {
            if (dmg > 0)
                _fx.CreatePopupText(dmg.ToString(), dmgColor);

            currentHp -= dmg;

            OnHealthChanged?.Invoke();
        }

        protected virtual void Die()
        {
        }

        private bool CanCrit()
        {
            int totalCritChance = critChance.ModifiedValue + agility.ModifiedValue;

            return Random.Range(0, 100) <= totalCritChance;
        }

        private int CalculateCriticalDamage(int dmg)
        {
            float totalCritPower = (critPower.ModifiedValue * 0.01f + strength.ModifiedValue * 0.01f);

            float critDamage = dmg * totalCritPower;


            return Mathf.RoundToInt(critDamage);
        }

        public int GetMaxHealthValue()
        {
            return maxHp.ModifiedValue + vitality.ModifiedValue * 5;
        }
        public void RecoverHP(int hpModify)
        {
            this.currentHp += hpModify;
            OnHealthChanged?.Invoke();
        }
    }
}