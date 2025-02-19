using Enemies;

public class EnemyStats : CharacterStats
{
    private Enemy _enemy;
    public EnemyStats(Stat maxHp) : base(maxHp)
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        _enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        _enemy.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        _enemy.Die();
    }
}