using System.Text;
using UnityEditor;
using UnityEngine;

namespace Inventory_and_Item
{
    public enum ItemType
    {
        Material,
        Equipment,
        Buff,
        Gold
    }
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public ItemType itemType;
        public string itemId;

        private void OnValidate()
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(this);
            itemId = AssetDatabase.AssetPathToGUID(path);
#endif
        }

        [Range(0, 100)]
        public float dropChance;
        protected StringBuilder sb = new StringBuilder();
        public virtual string GetDescription()
        {
            return "";
        }
    }
}