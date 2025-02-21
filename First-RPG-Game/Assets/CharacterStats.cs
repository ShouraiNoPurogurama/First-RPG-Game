using UnityEngine;
using UnityEngine.Serialization;

public class CharacterStats : MonoBehaviour
{
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

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;
    

    [SerializeField] private int currentHp;

    public CharacterStats(Stat maxHp)
    {
        this.maxHp = maxHp;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHp = maxHp.FinalValue;
        
        damage.AddModifier(4);
    }
    
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
            Debug.Log("CRIT DMG: " + totalDamage);
        }

        totalDamage = DecreaseDamageByArmor(targetStats, totalDamage);

        DoMagicalDamage(targetStats);
        // targetStats.TakeDamage(totalDamage);
    }

    public virtual void DoMagicalDamage(CharacterStats targetStats)
    {
        int fireDamageVal = fireDamage.FinalValue;
        int iceDamageVal = iceDamage.FinalValue;
        int lightingDamageVal = lightingDamage.FinalValue;

        int totalMagicalDamage = fireDamageVal + iceDamageVal + lightingDamageVal + intelligence.FinalValue;

        totalMagicalDamage = DecreaseDamageByResistance(targetStats, totalMagicalDamage);

        targetStats.TakeDamage(totalMagicalDamage);
    }

    private static int DecreaseDamageByResistance(CharacterStats targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= targetStats.magicResistance.FinalValue + (targetStats.intelligence.FinalValue * 3);

        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool ignite, bool chill, bool shock)
    {
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }

        isIgnited = ignite;
        isChilled = chill;
        isShocked = shock;
    }

    private static int DecreaseDamageByArmor(CharacterStats targetStats, int totalDamage)
    {
        totalDamage -= targetStats.armor.FinalValue;
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanDodgeAttack(CharacterStats targetStats)
    {
        int totalEvasion = targetStats.evasion.FinalValue + targetStats.agility.FinalValue;

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    public virtual void TakeDamage(int dmg)
    {
        currentHp -= dmg;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        
    }

    private bool CanCrit()
    {
        int totalCritChance = critChance.FinalValue + agility.FinalValue;

        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }

        return false;
    }

    private int CalculateCriticalDamage(int damage)
    {
        float totalCritPower = (critPower.FinalValue * 0.01f + strength.FinalValue * 0.01f);

        Debug.Log("Total crit power % " + totalCritPower);
        
        float critDamage = damage * totalCritPower;

        Debug.Log("Total crit damage  " + critDamage);
        
        return Mathf.RoundToInt(critDamage);
    }
}
