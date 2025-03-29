//using Enemies;
//using Inventory_and_Item;
//using UnityEngine;

//namespace Enemies.Wizard
//{
//    public class EnemyStats : CharacterStats
//    {
//        private Enemy _enemy;
//        private ItemDrop myDropSystem;
//        private EnemyWizard _wizard;

//        protected override void Start()
//        {
//            base.Start();
//            _enemy = GetComponent<Enemy>();
//            myDropSystem = GetComponent<ItemDrop>();
//            _wizard = GetComponent<EnemyWizard>();
//        }

//        public override void TakeDamage(int dmg, Color color = default)
//        {
//            if (isDead) return;
            
//            base.TakeDamage(dmg, color);
            
//            if (_wizard != null && !isDead)
//            {
//                _wizard.OnTakeHit();
//            }
//        }

//        protected override void Die()
//        {
//            if (isDead) return;
            
//            base.Die();
//            _enemy.Die();
//            myDropSystem.GenerateDrop();
//            Destroy(gameObject, 5f);
//        }
//    }
//}