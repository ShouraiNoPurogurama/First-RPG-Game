using MainCharacter;

public class PlayerStats : CharacterStats
{
    private Player _player;
    
    public PlayerStats(Stat maxHp) : base(maxHp)
    {
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        
        _player.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        
        _player.Die();
    }
}
