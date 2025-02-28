using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [SerializeField] public int currentHp;
    
    public Action OnHealthChanged;

    public CharacterStats(Stat maxHp)
    {
        this.maxHp = maxHp;
    }

    protected virtual void Start()
    {
        _fx= GetComponent<EntityFX>();
        
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
            DecreaseHealthBy(_igniteDamage);

            if (currentHp < 0)
            {
                Die();
            }

            _igniteDamageTimer = .3f;
        }
    }

    public void SetupIgniteDamage(int dmg) => _igniteDamage = dmg;

    public virtual void DoDamage(CharacterStats targetStats)
    {
        if (TargetCanDodgeAttack(targetStats))
        {
            return;
        }

        int totalDamage = damage.FinalValue + strength.FinalValue;

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = DecreaseDamageByArmor(targetStats, totalDamage);

        DoMagicalDamage(targetStats);
        
        targetStats.TakeDamage(totalDamage);
    }

    public virtual void DoMagicalDamage(CharacterStats targetStats)
    {
        int fireDamageVal = fireDamage.FinalValue;
        int iceDamageVal = iceDamage.FinalValue;
        int lightingDamageVal = lightingDamage.FinalValue;

        int totalMagicalDamage = fireDamageVal + iceDamageVal + lightingDamageVal + intelligence.FinalValue;

        totalMagicalDamage = DecreaseDamageByResistance(targetStats, totalMagicalDamage);

        targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(fireDamageVal, iceDamageVal, lightingDamageVal) <= 0)
        {
            return;
        }

        bool canApplyIgnite = fireDamageVal > iceDamageVal && fireDamageVal > lightingDamageVal;
        bool canApplyChill = iceDamageVal > fireDamageVal && iceDamageVal > lightingDamageVal;
        bool canApplyShock = lightingDamageVal > fireDamageVal && lightingDamageVal > iceDamageVal;

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

        targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private static int DecreaseDamageByResistance(CharacterStats targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= targetStats.magicResistance.FinalValue + targetStats.intelligence.FinalValue * 3;

        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool ignite, bool chill, bool shock)
    {
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }

        if (ignite)
        {
            isIgnited = true;
            _igniteTimer = ailmentDuration;
            
            _fx.IgniteFxFor(ailmentDuration);
        }

        if (chill)
        {
            isChilled = true;
            _chilledTimer = ailmentDuration;
            
            _fx.ChillFxFor(ailmentDuration);
        }

        if (shock)
        {
            isShocked = true;
            _shockedTimer = ailmentDuration;
            _fx.ShockFxFor(ailmentDuration);
        }

        isChilled = chill;
        isShocked = shock;
    }

    private static int DecreaseDamageByArmor(CharacterStats targetStats, int totalDamage)
    {
        if (targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(targetStats.armor.FinalValue * .7f);
        }
        else
        {
            totalDamage -= targetStats.armor.FinalValue;
        }

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);

        return totalDamage;
    }

    private bool TargetCanDodgeAttack(CharacterStats targetStats)
    {
        int totalEvasion = targetStats.evasion.FinalValue + targetStats.agility.FinalValue;

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


    public virtual void TakeDamage(int dmg)
    {
        DecreaseHPBy(dmg);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void DecreaseHealthBy(int dmg)
    {
        currentHp -= dmg;

        OnHealthChanged?.Invoke();
    }

    protected virtual void Die()
    {
    }

    private bool CanCrit()
    {
        int totalCritChance = critChance.FinalValue + agility.FinalValue;

        return Random.Range(0, 100) <= totalCritChance;
    }

    private int CalculateCriticalDamage(int dmg)
    {
        float totalCritPower = (critPower.FinalValue * 0.01f + strength.FinalValue * 0.01f);

        Debug.Log("Total crit power % " + totalCritPower);

        float critDamage = dmg * totalCritPower;


        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHp.FinalValue + vitality.FinalValue * 5;
    }
}