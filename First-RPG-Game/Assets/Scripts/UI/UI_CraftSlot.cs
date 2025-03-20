using Inventory_and_Item;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class Ui_CraftSlot : UiItemSlot
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        //[SerializeField] private UI_PopupCraft popupCraft;
        [SerializeField] private TextMeshProUGUI TextMeshProUGUI;
        private void OnEnable()
        {
            UpdateSlot(item);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
        
        }

        public void Craft()
        {
            ItemData_Equipment craftData = item.data as ItemData_Equipment;
            var check = Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
            if (!check)
            {
                TextMeshProUGUI.text = "Not enough materials";
                TextMeshProUGUI.color = Color.red;

            }
            else
            {
                TextMeshProUGUI.text = "Craft successfully!";
                TextMeshProUGUI.color = Color.green;
            }
        }

    

    }
}