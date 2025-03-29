using System.Collections.Generic;
using Save_and_Load;
using UI;
using UnityEditor;
using UnityEngine;

namespace Inventory_and_Item
{
    public class Inventory : MonoBehaviour, ISaveManager
    {
        public static Inventory instance;

        //Use for contain equipment and stash
        public List<InventoryItem> inventory;
        public Dictionary<ItemData, InventoryItem> inventoryDictionay;

        //Use for caft
        public List<InventoryItem> stash;
        public Dictionary<ItemData, InventoryItem> stashDictionary;

        //In equip UI
        public List<InventoryItem> equipment;
        public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

        public List<InventoryItem> startItem;

        [Header("Inventory UI")]
        [SerializeField] private Transform inventorySlotParent;
        [SerializeField] private Transform stashSlotParent;
        [SerializeField] private Transform equipmentSlotParent;
        [SerializeField] private Transform statSlotParent;

        [Header("DataBase")]
        public List<InventoryItem> loadedItems;
        public List<ItemData_Equipment> loadedEquipment;

        private UiItemSlot[] inventoryItemSlot;
        private UiItemSlot[] stashItemSlot;
        private UI_EquimentSlot[] equipmentItemSlot;
        private UI_StatSlot[] statSlots;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            inventory = new List<InventoryItem>();
            inventoryDictionay = new Dictionary<ItemData, InventoryItem>();

            stash = new List<InventoryItem>();
            stashDictionary = new Dictionary<ItemData, InventoryItem>();

            equipment = new List<InventoryItem>();
            equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

            inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UiItemSlot>();
            stashItemSlot = stashSlotParent.GetComponentsInChildren<UiItemSlot>();
            equipmentItemSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquimentSlot>();
            statSlots = statSlotParent.GetComponentsInChildren<UI_StatSlot>();
            //AddStartingItem();
        }

        private void AddStartingItem()
        {
            if (loadedItems.Count > 0)
            {
                foreach (InventoryItem item in loadedItems)
                {
                    for (int i = 0; i < item.stackSize; i++)
                    {
                        AddItem(item.data);
                    }
                }
            }
            else
            {
                // Nếu không có gì load thì thêm startItem mặc định
                for (int i = 0; i < startItem.Count; i++)
                {
                    AddItem(startItem[i].data);
                }
            }

            // Trang bị luôn các món equipment đã load được
            foreach (ItemData_Equipment item in loadedEquipment)
            {
                EquipItem(item);
            }
        }


        public void EquipItem(ItemData item)
        {
            ItemData_Equipment newEquipment = item as ItemData_Equipment;
            InventoryItem newItem = new InventoryItem(item);
            ItemData_Equipment oldItem = null;
            //check item in equipment ui
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> key in equipmentDictionary)
            {
                if (key.Key.equipmentType == newEquipment.equipmentType)
                {
                    oldItem = key.Key;
                }
            }
            //if esxist
            if (oldItem != null)
            {
                //unequip olditem from equipment ui
                UnEquipItem(oldItem);
                //back it to inventory Ui
                AddItem(oldItem);
            }

            //add item to equipment ui
            equipment.Add(newItem);
            equipmentDictionary.Add(newEquipment, newItem);
            //add information stat to player
            newEquipment.AddModifiers();
            //remove item from inventory
            RemoveItem(newEquipment);
            //update ui
            UpdateSlotUI();
        }

        public void UnEquipItem(ItemData_Equipment item)
        {
            if (equipmentDictionary.TryGetValue(item, out InventoryItem value))
            {
                equipment.Remove(value);
                equipmentDictionary.Remove(item);
                item.RemoveModifiers();
            }
        }

        private void UpdateSlotUI()
        {
            //Update UI in equip UI, if the type of equipment exist replace that equipment
            for (int i = 0; i < equipmentItemSlot.Length; i++)
            {
                foreach (KeyValuePair<ItemData_Equipment, InventoryItem> key in equipmentDictionary)
                {
                    if (key.Key.equipmentType == equipmentItemSlot[i].equipmentType)
                    {
                        equipmentItemSlot[i].UpdateSlot(key.Value);
                    }
                }
            }

            for (int i = 0; i < inventoryItemSlot.Length; i++)
            {
                inventoryItemSlot[i].CleanUpSlot();
            }

            for (int i = 0; i < inventoryItemSlot.Length; i++)
            {
                stashItemSlot[i].CleanUpSlot();
            }


            for (int i = 0; i < inventory.Count; i++)
            {
                inventoryItemSlot[i].UpdateSlot(inventory[i]);
            }

            for (int i = 0; i < stash.Count; i++)
            {
                stashItemSlot[i].UpdateSlot(stash[i]);
            }

            for (int i = 0; i < statSlots.Length; i++)
            {
                statSlots[i].UpdateStatValueUI();
            }
        }
        public bool CanAddItem()
        {
            if (inventory.Count >= inventoryItemSlot.Length)
            {
                return false;
            }

            return true;
        }
        public void AddItem(ItemData item)
        {
            if (item.itemType == ItemType.Equipment)
            {
                AddToInventory(item);
            }
            else if (item.itemType == ItemType.Material)
            {
                AddToStash(item);
            }
            else if (item.itemType == ItemType.Buff)
            {
                ItemData_Buff itemData = (ItemData_Buff)item;
                itemData.AddModifiers();
            }
            else if (item.itemType == ItemType.Gold)
            {
                ItemDataGold itemData = (ItemDataGold)item;
                itemData.AddGold();
            }

            UpdateSlotUI();
        }
        private void AddToStash(ItemData item)
        {
            if (stashDictionary.TryGetValue(item, out InventoryItem value))
            {
                value.AddStack();
            }
            else
            {
                InventoryItem inventoryItem = new InventoryItem(item);
                stash.Add(inventoryItem);
                stashDictionary.Add(item, inventoryItem);
            }
        }
        private void AddToInventory(ItemData item)
        {
            if (inventoryDictionay.TryGetValue(item, out InventoryItem value))
            {
                value.AddStack();
            }
            else
            {
                InventoryItem inventoryItem = new InventoryItem(item);
                inventory.Add(inventoryItem);
                inventoryDictionay.Add(item, inventoryItem);
            }
        }

        public void RemoveItem(ItemData item)
        {
            if (item.itemType == ItemType.Equipment)
            {
                RemoveInventory(item);
            }
            else
            {
                RemoveStash(item);
            }

            UpdateSlotUI();
        }

        private void RemoveStash(ItemData item)
        {
            if (stashDictionary.TryGetValue(item, out InventoryItem stashValue))
            {
                if (stashValue.stackSize <= 1)
                {
                    stash.Remove(stashValue);
                    stashDictionary.Remove(item);
                }
                else
                    stashValue.RemoveStack();
            }
        }

        private void RemoveInventory(ItemData item)
        {
            if (inventoryDictionay.TryGetValue(item, out InventoryItem value))
            {
                if (value.stackSize <= 1)
                {
                    inventory.Remove(value);
                    inventoryDictionay.Remove(item);
                }
                else
                    value.RemoveStack();
            }
        }

        public bool CanCraft(ItemData_Equipment itemToCraft, List<InventoryItem> requiredMaterials)
        {
            List<InventoryItem> materialsToRemove = new List<InventoryItem>();
            for (int i = 0; i < requiredMaterials.Count; i++)
            {
                if (stashDictionary.TryGetValue(requiredMaterials[i].data, out InventoryItem stashValue))
                {
                    if (stashValue.stackSize < requiredMaterials[i].stackSize)
                    {
                        Debug.Log("Not enough materials");
                        return false;
                    }
                    else
                    {
                        materialsToRemove.Add(stashValue);
                    }
                }
                else
                {
                    Debug.Log("Not enough materials");
                    return false;
                }
            }

            for (int i = 0; i < materialsToRemove.Count; i++)
            {
                RemoveItem(materialsToRemove[i].data);
            }

            AddItem(itemToCraft);
            Debug.Log("Here is your item " + itemToCraft.name);
            return true;
        }
        public List<InventoryItem> GetEquipmentList() => equipment;
        public List<InventoryItem> GetStashList() => stash;
        public ItemData_Equipment GetEquipment(EquipmentType type)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> key in equipmentDictionary)
            {
                if (key.Key.equipmentType == type)
                {
                    return key.Key;
                }
            }
            return null;
        }
#if UNITY_EDITOR
        public void LoadData(GameData _data)
        {
            foreach (KeyValuePair<string, int> pair in _data.inventory)
            {
#if UNITY_EDITOR
                foreach (var item in GetItemDataBase())
                {
                    if (item != null && item.itemId == pair.Key)
                    {
                        InventoryItem itemToLoad = new InventoryItem(item);
                        itemToLoad.stackSize = pair.Value;

                        loadedItems.Add(itemToLoad);
                    }
                }
#endif
            }

            foreach (string loadedItemId in _data.equipmentId)
            {
#if UNITY_EDITOR
                foreach (var item in GetItemDataBase())
                {
                    if (item != null && loadedItemId == item.itemId)
                    {
                        loadedEquipment.Add(item as ItemData_Equipment);
                    }
                }
#endif
            }
            AddStartingItem();
        }

        public void SaveData(ref GameData _data)
        {
            _data.inventory.Clear();
            _data.equipmentId.Clear();

            foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionay)
            {
                _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
            }

            foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
            {
                _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
            }
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
            {
                _data.equipmentId.Add(pair.Key.itemId);
            }
        }
        
        private List<ItemData> GetItemDataBase()
        {
            List<ItemData> itemDataBase = new List<ItemData>();

            string[] assetName = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });

            foreach (string guid in assetName)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);
                itemDataBase.Add(item);
            }
            return itemDataBase;
        }
#endif
    }
}