using System.Collections.Generic;
using UnityEngine;

namespace Inventory_and_Item
{
    public class ItemDrop : MonoBehaviour
    {
        [SerializeField] private int possibleItemDrop;
        [SerializeField] private ItemData[] possibleDrop;
        private List<ItemData> dropList = new List<ItemData>();

        [SerializeField] private GameObject dropPrefab;

        public void GenerateDrop()
        {
            dropList.Clear();

            // Add items to dropList based on dropChance
            foreach (var item in possibleDrop)
            {
                if (Random.Range(0, 100) <= item.dropChance)
                {
                    dropList.Add(item);
                }
            }

            // Prevent errors when dropList is empty
            if (dropList.Count == 0)
            {
                return;
            }

            int dropsToGenerate = Mathf.Min(possibleItemDrop, dropList.Count);

            for (int i = 0; i < dropsToGenerate; i++)
            {
                int randomIndex = Random.Range(0, dropList.Count);
                ItemData randomItem = dropList[randomIndex];
                dropList.RemoveAt(randomIndex);
                DropItem(randomItem);
            }

        }

        public void DropItem(ItemData itemdata)
        {
            GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
            Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(12, 15));
            newDrop.GetComponent<ItemObject>().SetupItem(itemdata, randomVelocity);
        }
    }
}