using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionay;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> startItem;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;

    private Ui_ItemSlot[] inventoryItemSlot;
    private Ui_ItemSlot[] stashItemSlot;
    private UI_EquimentSlot[] equipmentItemSlot;
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

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<Ui_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<Ui_ItemSlot>();
        equipmentItemSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquimentSlot>();
        AddStartingItem();
    }

    private void AddStartingItem()
    {
        for(int i = 0; i < startItem.Count; i++)
        {
            AddItem(startItem[i].data);
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
            if(key.Key.equipmentType == newEquipment.equipmentType)
            {
                oldItem = key.Key;
            }
        }
        //if esxist
        if(oldItem != null)
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
    }
    public void AddItem(ItemData item)
    {
        if(item.itemType == ItemType.Equipment)
        {
            AddToInventory(item);
        } else
        {
            AddToStash(item);
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
                if(stashValue.stackSize < requiredMaterials[i].stackSize)
                {
                    Debug.Log("Not enough materials");
                    return false;
                } else
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

        for (int i = 0; i < materialsToRemove.Count;i++)
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
}
