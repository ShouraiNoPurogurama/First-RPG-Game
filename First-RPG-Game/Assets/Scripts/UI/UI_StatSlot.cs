using MainCharacter;
using Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UI_StatSlot : MonoBehaviour
    {
        [SerializeField] private string stateName;
        [SerializeField] private StatType statType;
        [SerializeField] private TextMeshProUGUI statValueText;
        [SerializeField] private TextMeshProUGUI statNameText;

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
            if (PlayerManager.Instance.player.GetComponent<PlayerStats>().Gold < 50)
            {
                return;
            }
            PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
            playerStats.GetStat(statType).AddModifier(1);
            PlayerManager.Instance.player.GetComponent<PlayerStats>().Gold -= 50;
            UpdateStatValueUI();
        }
    }
}
