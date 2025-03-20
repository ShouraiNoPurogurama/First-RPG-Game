using Inventory_and_Item;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UI_ToolTip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textName;
        [SerializeField] private TextMeshProUGUI textType;
        [SerializeField] private TextMeshProUGUI textDescription;

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void ShowToolTip(ItemData itemData)
        {
            if (itemData == null)
            {
                return;
            }
            textName.text = itemData.itemName;
            textType.text = itemData.itemType.ToString();
            textDescription.text = itemData.GetDescription();
            gameObject.SetActive(true);
        }

        public void HideToolTip() => gameObject.SetActive(false);
    }
}
