using MainCharacter;
using UnityEngine;

namespace Inventory_and_Item
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Vector2 velocity;
        private void OnValidate()
        {
            SetUpVisuals();
        }

        private void SetUpVisuals()
        {
            if (itemData == null)
            {
                return;
            }
            GetComponent<SpriteRenderer>().sprite = itemData.icon;
            gameObject.name = "Item object - " + itemData.name;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                rb.linearVelocity = velocity;
            }
        }

        public void SetupItem(ItemData itemData, Vector2 velocity)
        {
            this.itemData = itemData;
            this.velocity = velocity;
            SetUpVisuals();
        }

        public void PickItem(Collider2D collision)
        {
            if (collision.GetComponent<Player>() != null)
            {
                Inventory.instance.AddItem(itemData);
                Destroy(gameObject);
            }
        }
    }
}