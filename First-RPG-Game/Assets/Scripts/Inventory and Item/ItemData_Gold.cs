using MainCharacter;
using Stats;
using UnityEngine;
namespace Assets.Scripts.Inventory_and_Item
{
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Gold")]
    public class ItemData_Gold : ItemData
    {
        public int goldAmount;
        public void AddGold()
        {
            // Add gold to player's inventory
            PlayerManager.Instance.player.GetComponent<PlayerStats>().Gold += goldAmount;
        }
    }
}
