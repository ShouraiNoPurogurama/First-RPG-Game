using UnityEngine;
using UnityEngine.EventSystems;

public class Ui_CraftSlot : Ui_ItemSlot
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.data as ItemData_Equipment;
        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
    }
}