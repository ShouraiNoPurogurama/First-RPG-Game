using MainCharacter;
using Stats;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UI_StatSlot : MonoBehaviour
    {
        [SerializeField] private string stateName;
        [SerializeField] private StatType statType;
        [SerializeField] private TextMeshProUGUI statValueText;
        [SerializeField] private TextMeshProUGUI statNameText;

        [Header("Souls info")]
        [SerializeField] private float goldAmount;
        [SerializeField] private float increaseRate = 100;

        private void OnValidate()
        {
            gameObject.name = "Stat - " + stateName;
            if (stateName != null)
            {
                statNameText.text = stateName;
            }
        }

        void Start()
        {
            UpdateStatValueUI();
        }

        public void UpdateStatValueUI()
        {
            PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
            if (statValueText != null)
            {
                statValueText.text = playerStats.GetStat(statType).GetValue().ToString();
            }
        }
        public void Upgrade()
        {
            UpdateGold();

            if (PlayerManager.Instance.player.GetComponent<PlayerStats>().Gold < 50)
            {
                return;
            }
            PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
            playerStats.GetStat(statType).AddModifier(1);
            PlayerManager.Instance.player.GetComponent<PlayerStats>().Gold -= 50;
            UpdateStatValueUI();
        }

        private void UpdateGold()
        {
            if (goldAmount < PlayerManager.Instance.player.GetComponent<PlayerStats>().Gold)
            {
                goldAmount += Time.deltaTime * increaseRate;
            }
            else
            {
                goldAmount = PlayerManager.Instance.player.GetComponent<PlayerStats>().Gold;
            }
        }
    }
}
