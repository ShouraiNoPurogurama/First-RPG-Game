using Inventory_and_Item;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UiItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemText;

        private UI ui;
        public InventoryItem item;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            ui = GetComponentInParent<UI>();
        }
        public void UpdateSlot(InventoryItem item)
        {
            this.item = item;
            itemImage.color = Color.white;
            if (item != null)
            {
                itemImage.sprite = item.data.icon;
                if (item.stackSize > 1)
                {
                    itemText.text = item.stackSize.ToString();
                }
                else
                {
                    itemText.text = "";
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CleanUpSlot()
        {
            item = null;
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            itemText.text = "";
        }
        public virtual void OnPointerDown(PointerEventData eventData)
        {

            if (item.data.itemType == ItemType.Equipment)
            {
                Inventory.instance.EquipItem(item.data);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ui?.toolTipUI?.HideToolTip();

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (item == null)
            {
                return;
            }
            ui?.toolTipUI?.ShowToolTip(item.data);
        }
    }
}