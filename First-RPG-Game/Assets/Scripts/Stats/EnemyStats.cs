using Enemies;
using UnityEngine;

namespace Stats
{
    public class EnemyStats : CharacterStats
    {
        private Enemy _enemy;
        private ItemDrop myDropSystem;

        public EnemyStats(Stat maxHp) : base(maxHp)
        {
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            _enemy = GetComponent<Enemy>();
            myDropSystem = GetComponent<ItemDrop>();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        public override void TakeDamage(int dmg, Color? color = null)
        {
            base.TakeDamage(dmg, color);
        }

        protected override void Die()
        {
            base.Die();
            _enemy.Die();
            myDropSystem.GenerateDrop();
        }
    }
}