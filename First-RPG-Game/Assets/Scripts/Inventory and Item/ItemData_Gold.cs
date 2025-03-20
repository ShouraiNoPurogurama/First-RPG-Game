using MainCharacter;
using Stats;
using UnityEngine;

namespace Inventory_and_Item
{
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Gold")]
    public class ItemDataGold : ItemData
    {
        public int goldAmount;
        public void AddGold()
        {
            // Add gold to player's inventory
            PlayerManager.Instance.player.GetComponent<PlayerStats>().Gold += goldAmount;
        }
    }
}
