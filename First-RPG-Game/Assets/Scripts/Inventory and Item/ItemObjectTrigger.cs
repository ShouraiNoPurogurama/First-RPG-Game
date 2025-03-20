using MainCharacter;
using UnityEngine;

namespace Inventory_and_Item
{
    public class ItemObjectTrigger : MonoBehaviour
    {
        private ItemObject myItemObject => GetComponentInParent<ItemObject>();
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>() != null)
            {
                myItemObject.PickItem(collision);
            }

        }
    }
}