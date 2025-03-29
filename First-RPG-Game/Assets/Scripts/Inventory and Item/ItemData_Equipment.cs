using System.Collections.Generic;
using MainCharacter;
using Stats;
using UnityEngine;

namespace Inventory_and_Item
{
    public enum EquipmentType
    {
        Element,
        Weapon,
        Armor
    }

    [CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
    public class ItemData_Equipment : ItemData
    {
        public EquipmentType equipmentType;

        public ItemEffect.ItemEffect[] itemEffects;

        [Header("Major stats")]
        public int strength; // = 1 dmg and 1% crit chance
        public int agility; // = 1% evasion and 1% crit chance
        public int intelligence; // = 1 magic dmg and 3% magic resistance
        public int vitality; //= 5 health

        [Header("Defensive stats")]
        public int maxHp;
        public int armor;
        public int evasion;
        public int magicResistance;

        [Header("Offensive stats")]
        public int damage;
        public int critChance;
        public int critPower;

        [Header("Magic stats")]
        public int fireDamage;
        public int iceDamage;
        public int lightingDamage;
        public int earthDamage;
        public int windDamage;

        [Header("Craft requirements")]
        public List<InventoryItem> craftingMaterials;


        public void RemoveModifiers()
        {
            PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
            playerStats.strength.RemoveModifier(strength);
            playerStats.agility.RemoveModifier(agility);
            playerStats.intelligence.RemoveModifier(intelligence);
            playerStats.vitality.RemoveModifier(vitality);


            playerStats.damage.RemoveModifier(damage);
            playerStats.critChance.RemoveModifier(critChance);
            playerStats.critPower.RemoveModifier(critPower);


            playerStats.maxHp.RemoveModifier(maxHp);
            playerStats.armor.RemoveModifier(armor);
            playerStats.evasion.RemoveModifier(evasion);
            playerStats.magicResistance.RemoveModifier(magicResistance);


            playerStats.fireDamage.RemoveModifier(fireDamage);
            playerStats.iceDamage.RemoveModifier(iceDamage);
            playerStats.lightingDamage.RemoveModifier(lightingDamage);
            playerStats.earthDamage.RemoveModifier(lightingDamage);
            playerStats.windDamage.RemoveModifier(lightingDamage);
        }

        public void AddModifiers()
        {
            PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

            playerStats.strength.AddModifier(strength);
            playerStats.agility.AddModifier(agility);
            playerStats.intelligence.AddModifier(intelligence);
            playerStats.vitality.AddModifier(vitality);

            playerStats.damage.AddModifier(damage);
            playerStats.critChance.AddModifier(critChance);
            playerStats.critPower.AddModifier(critPower);

            playerStats.maxHp.AddModifier(maxHp);
            playerStats.armor.AddModifier(armor);
            playerStats.evasion.AddModifier(evasion);
            playerStats.magicResistance.AddModifier(magicResistance);

            playerStats.fireDamage.AddModifier(fireDamage);
            playerStats.iceDamage.AddModifier(iceDamage);
            playerStats.lightingDamage.AddModifier(lightingDamage);
            playerStats.windDamage.AddModifier(lightingDamage);
            playerStats.earthDamage.AddModifier(lightingDamage);
        }

        public void Effect(Transform enemyPosition)
        {
            foreach (var item in itemEffects)
            {
                item.ExecuteEffect(enemyPosition);
            }
        }

        public override string GetDescription()
        {
            sb.Length = 0;

            AddItemDescription(strength, "Strength");
            AddItemDescription(agility, "Agility");
            AddItemDescription(intelligence, "Intelligence");
            AddItemDescription(vitality, "Vitality");

            AddItemDescription(damage, "Damage");
            AddItemDescription(critChance, "Crit.Chance");
            AddItemDescription(critPower, "Crit.Power");

            AddItemDescription(maxHp, "Hp");
            AddItemDescription(evasion, "Evasion");
            AddItemDescription(armor, "Armor");
            AddItemDescription(magicResistance, "Magic Resist.");

            AddItemDescription(fireDamage, "Fire damage");
            AddItemDescription(iceDamage, "Ice damage");
            AddItemDescription(lightingDamage, "Lighting dmg. ");
            AddItemDescription(earthDamage, "Earth dmg. ");
            AddItemDescription(windDamage, "Wind dmg. ");

            return sb.ToString();
        }



        private void AddItemDescription(int _value, string _name)
        {
            if (_value != 0)
            {
                if (sb.Length > 0)
                    sb.AppendLine();
                if (_value > 0)
                    sb.Append("+ " + _value + " " + _name);
            }
        }
    }
}