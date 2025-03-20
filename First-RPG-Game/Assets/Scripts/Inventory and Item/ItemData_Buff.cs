using MainCharacter;
using Stats;
using UnityEngine;

namespace Inventory_and_Item
{
    public enum BuffType
    {
        Weapon,
        Armor,
        Amulet,
        Flask
    }

    [CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Buff")]
    public class ItemData_Buff : ItemData
    {
        [Header("Major stats")]
        public int hpPlus; // 

        public void AddModifiers()
        {
            PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
            playerStats.RecoverHPBy(hpPlus);
        }
    }
}