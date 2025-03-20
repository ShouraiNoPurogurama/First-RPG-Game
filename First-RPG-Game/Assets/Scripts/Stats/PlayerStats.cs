using MainCharacter;
using UnityEngine;

namespace Stats
{
    public class PlayerStats : CharacterStats
    {
        public int Gold;
        public int Ruby;
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

        public override void TakeDamage(int dmg, Color color = default)
        {
            if (color == default) color = Color.white;
            
            base.TakeDamage(dmg, color);
        }

        protected override void Die()
        {
            base.Die();
            GetComponent<Collider2D>().enabled = false; // Hide player Die
            _player.Die();
        }
    }
}