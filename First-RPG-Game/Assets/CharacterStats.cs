using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // public int damage;
    public Stat maxHp;

    public Stat damage;
    public Stat strength;

    [SerializeField] private int currentHp;

    public CharacterStats(Stat maxHp)
    {
        this.maxHp = maxHp;
    }

    public virtual void DoDamage(CharacterStats targetStats)
    {

        int totalDamage = damage.FinalValue + strength.FinalValue;
        targetStats.TakeDamage(totalDamage);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        currentHp = maxHp.FinalValue;
        
        Debug.Log("Adding modifier");
        
        //Example equip sword with 4 damage
        damage.AddModifier(4);
    }

    public virtual void TakeDamage(int dmg)
    {
        currentHp -= dmg;

        if (currentHp < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        
    }
}
