using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquimentSlot : Ui_ItemSlot
{
    public EquipmentType equipmentType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Inventory.instance.UnEquipItem((ItemData_Equipment)item.data);
        Inventory.instance.AddItem(item.data);
        CleanUpSlot();
    }

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + equipmentType.ToString();
    }
}
