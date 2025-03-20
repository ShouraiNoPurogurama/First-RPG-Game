using Inventory_and_Item;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_ListMaterial : MonoBehaviour
    {
        [SerializeField] private Ui_CraftSlot Ui_CraftSlot;
        private Image[] Images;
        public void Start()
        {
            Images = GetComponentsInChildren<Image>();
            ItemData_Equipment craftData = Ui_CraftSlot.item.data as ItemData_Equipment;
            for (int i = 0; i < craftData.craftingMaterials.Count; i++)
            {
                Images[i].sprite = craftData.craftingMaterials[i].data.icon;
            }
        }

    }
}
