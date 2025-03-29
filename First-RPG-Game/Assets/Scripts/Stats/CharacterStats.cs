using Enemies;
using MainCharacter;
using UI;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Stats
{
    public enum StatType
    {
        strength,
        agility,
        intelegence,
        vitality,
        damage,
        critChance,
        critPower,
        health,
        armor,
        evasion,
        magicRes,
        fireDamage,
        iceDamage,
        lightingDamage,
        Gold
    }

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
        public Stat earthDamage;
        public Stat windDamage;

        public bool isIgnited; //does target's damage over time
        public bool isChilled; // reduce target's armor by 30%
        public bool isShocked; // reduce target's attack accuracy by 20%
        public bool isEarthAffected; // reduce target's movement speed by 50% and decrease evasion by 40% 
        public bool isWindAffected; // reduce target's attack speed by 30% and reduce critical chance by 20%

        [SerializeField] protected float ailmentDuration = 4;
        private float _igniteTimer;
        private float _chilledTimer;
        private float _shockedTimer;
        private float _earthTimer;
        private float _windTimer;

        private float _igniteDamageCooldown = .5f;
        private float _igniteDamageTimer = .5f;
        private int _igniteDamage;
        private int _shockDamage;

        [SerializeField] private GameObject ShockEffectPrefab;

        [SerializeField] public int currentHp;

        public System.Action OnHPChanged;
        public bool isInvincible { get; private set; }
        public bool isDead { get; private set; }

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
            _earthTimer -= Time.deltaTime;
            _windTimer -= Time.deltaTime;


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

            if (_earthTimer <= 0)
            {
                isEarthAffected = false;
                GetComponent<CharacterStats>()
                    .evasion.RemoveModifier(Mathf.RoundToInt(-GetComponent<CharacterStats>().evasion.GetValue() * 0.4f));
            }

            if (_windTimer <= 0)
            {
                isWindAffected = false;
                GetComponent<CharacterStats>()
                    .critChance.RemoveModifier(Mathf.RoundToInt(-GetComponent<CharacterStats>().critChance.GetValue() * 0.2f));
            }

            if (_igniteDamageTimer <= 0 && isIgnited)
            {
                DecreaseHPBy(_igniteDamage);

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

            targetStats.GetComponent<Entity>().SetupKnockBackDir(transform); //Calculate knockback direction

            int totalDamage = damage.ModifiedValue + strength.ModifiedValue;


            if (CanCrit())
            {
                totalDamage = CalculateCriticalDamage(totalDamage);
            }

            totalDamage = DecreaseDamageByArmor(targetStats, totalDamage);

            Debug.Log("Total damage: " + totalDamage);

            //If equipments have ailment effects then do magical damage
            // DoMagicalDamage(targetStats);

            targetStats.TakeDamage(totalDamage, Color.red);
        }
        public virtual void DoDamageDontKnock(CharacterStats targetStats)
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

            Debug.Log("Total damage: " + totalDamage);

            //If equipments have ailment effects then do magical damage
            // DoMagicalDamage(targetStats);

            targetStats.TakeDamageNoImpact(totalDamage, Color.red);
        }

        public virtual void DoMagicalDamage(CharacterStats targetStats, float dmgScale = 1)
        {
            int fireDamageVal = fireDamage.ModifiedValue;
            int iceDamageVal = iceDamage.ModifiedValue;
            int lightingDamageVal = lightingDamage.ModifiedValue;
            int earthDamageVal = earthDamage.ModifiedValue;
            int windDamageVal = windDamage.ModifiedValue;

            int totalMagicalDamage = fireDamageVal + iceDamageVal + lightingDamageVal + windDamageVal + earthDamageVal +
                                     intelligence.ModifiedValue;

            totalMagicalDamage = DecreaseDamageByResistance(targetStats, totalMagicalDamage);

            if (Mathf.Max(fireDamageVal, iceDamageVal, lightingDamageVal, earthDamageVal, windDamageVal) <= 0)
            {
                return;
            }

            // Setup Color
            Color damageColor = Color.white;
            if (fireDamageVal >= iceDamageVal && fireDamageVal >= lightingDamageVal && fireDamageVal >= earthDamageVal && fireDamageVal >= windDamageVal)
                damageColor = Color.red;
            else if (iceDamageVal >= fireDamageVal && iceDamageVal >= lightingDamageVal && iceDamageVal >= earthDamageVal && iceDamageVal >= windDamageVal)
                damageColor = Color.blue;
            else if (lightingDamageVal >= fireDamageVal && lightingDamageVal >= iceDamageVal && lightingDamageVal >= earthDamageVal && lightingDamageVal >= windDamageVal)
                damageColor = Color.yellow;
            else if (earthDamageVal >= fireDamageVal && earthDamageVal >= iceDamageVal && earthDamageVal >= lightingDamageVal && earthDamageVal >= windDamageVal)
                damageColor = new Color(0.545f, 0.271f, 0.075f); //(brown)
            else if (windDamageVal >= fireDamageVal && windDamageVal >= iceDamageVal && windDamageVal >= lightingDamageVal && windDamageVal >= earthDamageVal)
                damageColor = Color.green;

            //Use sort instead
            bool canApplyIgnite = fireDamageVal > iceDamageVal && fireDamageVal > lightingDamageVal &&
                                  fireDamageVal > earthDamageVal && fireDamageVal > windDamageVal;
            bool canApplyChill = iceDamageVal > fireDamageVal && iceDamageVal > lightingDamageVal &&
                                 iceDamageVal > earthDamageVal && iceDamageVal > windDamageVal;
            bool canApplyShock = lightingDamageVal > fireDamageVal && lightingDamageVal > iceDamageVal &&
                                 lightingDamageVal > earthDamageVal && lightingDamageVal > windDamageVal;
            bool canApplyEarth = earthDamageVal > fireDamageVal && earthDamageVal > iceDamageVal &&
                                 earthDamageVal > lightingDamageVal && earthDamageVal > windDamageVal;
            bool canApplyWind = windDamageVal > fireDamageVal && windDamageVal > iceDamageVal &&
                                windDamageVal > lightingDamageVal && windDamageVal > earthDamageVal;

            targetStats.TakeDamage(Mathf.RoundToInt(totalMagicalDamage * dmgScale), damageColor);

            while (!canApplyIgnite && !canApplyChill && !canApplyShock && !canApplyWind && !canApplyEarth)
            {
                if (Random.value < .5f && fireDamageVal > 0)
                {
                    canApplyIgnite = true;
                    targetStats.ApplyAilments(true, false, false, false, false);
                    return;
                }

                if (Random.value < .5f && iceDamageVal > 0)
                {
                    canApplyChill = true;
                    targetStats.ApplyAilments(false, true, false, false, false);
                    return;
                }

                if (Random.value < .5f && lightingDamageVal > 0)
                {
                    canApplyShock = true;
                    targetStats.ApplyAilments(false, false, true, false, false);
                    return;
                }

                if (Random.value < .5f && earthDamageVal > 0)
                {
                    canApplyEarth = true;
                    targetStats.ApplyAilments(false, false, false, true, false);
                    return;
                }

                if (Random.value < .5f && windDamageVal > 0)
                {
                    canApplyWind = true;
                    targetStats.ApplyAilments(false, false, false, false, true);
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

            targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock, canApplyEarth, canApplyWind);
        }

        private static int DecreaseDamageByResistance(CharacterStats targetStats, int totalMagicalDamage)
        {
            totalMagicalDamage -= targetStats.magicResistance.ModifiedValue + targetStats.intelligence.ModifiedValue * 3;

            totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
            return totalMagicalDamage;
        }

        public void ApplyAilments(bool ignite, bool chill, bool shock, bool earth, bool wind)
        {
            bool canApplyIgnite = !isIgnited && !isChilled && !isShocked && !isEarthAffected && !isWindAffected;
            bool canApplyChill = !isIgnited && !isChilled && !isShocked && !isEarthAffected && !isWindAffected;
            bool canApplyShock = !isIgnited && !isChilled && !isEarthAffected && !isWindAffected;
            bool canApplyEarth = !isIgnited && !isChilled && !isShocked && !isEarthAffected && !isWindAffected;
            bool canApplyWind = !isIgnited && !isChilled && !isShocked && !isEarthAffected && !isWindAffected;

            if (isIgnited || isChilled || isEarthAffected || isWindAffected)
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

            //TODO : Add earth and wind effects
            if (earth && canApplyEarth)
            {
                isEarthAffected = true;
                _earthTimer = ailmentDuration;

                var slowPercent = 0.5f;
                var evasionReductionPercent = 0.4f;
                GetComponent<Entity>().SlowEntityBy(slowPercent, ailmentDuration);
                GetComponent<CharacterStats>()
                    .evasion.AddModifier(Mathf.RoundToInt(-GetComponent<CharacterStats>().evasion.GetValue() *
                                                          evasionReductionPercent));

                _fx.EarthFxFor(ailmentDuration);
            }

            if (wind && canApplyWind)
            {
                isWindAffected = true;
                _windTimer = ailmentDuration;

                var attackSpeedReductionPercent = 0.3f;
                var critChanceReductionPercent = 0.2f;
                GetComponent<Entity>().ReduceAttackSpeedBy(attackSpeedReductionPercent, ailmentDuration);
                GetComponent<CharacterStats>()
                    .critChance.AddModifier(Mathf.RoundToInt(-GetComponent<CharacterStats>().critChance.GetValue() *
                                                             critChanceReductionPercent));

                _fx.WindFxFor(ailmentDuration);
            }


            isChilled = chill;
            isShocked = shock;
            isEarthAffected = earth;
            isWindAffected = wind;
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

        public virtual void TakeDamage(int dmg, Color color = default)
        {
            if (isInvincible)
                return;
            if (color == default) color = Color.white;
            GetComponent<Entity>().DamageImpact();

            _fx.Flash();

            DecreaseHPBy(dmg);

            if (dmg > 0) _fx.CreatePopUpText(dmg.ToString(), color);

            if (currentHp <= 0)
            {
                Die();
            }
        }
        public virtual void TakeDamageNoImpact(int dmg, Color color = default)
        {
            if (color == default) color = Color.white;

            _fx.Flash();

            DecreaseHPBy(dmg);

            if (dmg > 0) _fx.CreatePopUpText(dmg.ToString(), color);

            if (currentHp <= 0)
            {
                Die();
            }
        }

        protected virtual void DecreaseHPBy(int dmg)
        {
            currentHp -= dmg;

            if (dmg > 0) _fx.CreatePopUpText(dmg.ToString(), Color.red);

            OnHPChanged?.Invoke();
        }

        protected virtual void Die()
        {
            isDead = true;
            currentHp = 0;
        }

        public void KillEntity()
        {
            if (!isDead)
            {
                Die();
            }
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

        public void RecoverHPBy(int hpModify)
        {
            currentHp += hpModify;
            OnHPChanged?.Invoke();
        }

        public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

        public Stat GetStat(StatType _statType)
        {
            if (_statType == StatType.strength) return strength;
            else if (_statType == StatType.agility) return agility;
            else if (_statType == StatType.intelegence) return intelligence;
            else if (_statType == StatType.vitality) return vitality;
            else if (_statType == StatType.damage) return damage;
            else if (_statType == StatType.critChance) return critChance;
            else if (_statType == StatType.critPower) return critPower;
            else if (_statType == StatType.health) return maxHp;
            else if (_statType == StatType.armor) return armor;
            else if (_statType == StatType.evasion) return evasion;
            else if (_statType == StatType.magicRes) return magicResistance;
            else if (_statType == StatType.fireDamage) return fireDamage;
            else if (_statType == StatType.iceDamage) return iceDamage;
            else if (_statType == StatType.lightingDamage) return lightingDamage;

            return null;
        }
    }
}