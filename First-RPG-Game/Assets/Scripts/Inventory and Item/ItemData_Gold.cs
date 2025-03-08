using MainCharacter;
using Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
