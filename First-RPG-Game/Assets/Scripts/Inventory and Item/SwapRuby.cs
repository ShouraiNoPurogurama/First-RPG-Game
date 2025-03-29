using System.Threading.Tasks;
using MainCharacter;
using Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory_and_Item
{
    public class SwapRuby : MonoBehaviour
    {
        [SerializeField] private int Gold;
        [SerializeField] private int Ruby;
        public Button swapButton; // Kéo button từ Inspector vào đây

        private void Start()
        {
            if (swapButton != null)
            {
                swapButton.onClick.AddListener(OnSwapButtonClicked);
            }
        }

        private async void OnSwapButtonClicked()
        {
            if (swapButton != null)
            {
                swapButton.interactable = false; // Tắt button để tránh spam
            }

            await Swap();

            if (swapButton != null)
            {
                swapButton.interactable = true; // Bật lại sau khi xong
            }
        }

        public async Task Swap()
        {
            try
            {
                await APITrigger.Instance.LoadRubyDB();

                var playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
                if (playerStats.Ruby < Ruby)
                {
                    Debug.Log("Không đủ Ruby để đổi vàng!");
                    return;
                }

                playerStats.Ruby -= Ruby;
                playerStats.Gold += Gold;

                await APITrigger.Instance.SaveRuby();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"🔴 Lỗi khi Swap: {ex.Message}");
            }
        }
    }
}
