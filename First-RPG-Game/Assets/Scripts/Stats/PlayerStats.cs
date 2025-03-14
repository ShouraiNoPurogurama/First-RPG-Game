using MainCharacter;
using UnityEngine;

namespace Stats
{
    public class PlayerStats : CharacterStats
    {
        public int Gold;
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
        protected override void Update()
        {
            base.Update();
        }

        public override void TakeDamage(int dmg, Color dmgColor = default)
        {
            base.TakeDamage(dmg, dmgColor);
        }

        protected override void Die()
        {
            base.Die();
        
            _player.Die();
        }
    }
}
