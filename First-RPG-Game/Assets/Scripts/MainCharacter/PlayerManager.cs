using Save_and_Load;
using Stats;
using UnityEngine;

namespace MainCharacter
{
    public class PlayerManager : MonoBehaviour, ISaveManager
    {
        public static PlayerManager Instance;
        public Player player;

        private void Awake()
        {
            //When have multiple scenes
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadData(GameData _data)
        {
            SetData(_data);
        }
        /// <summary>
        /// Save player data
        /// </summary>
        /// <param name="_data"></param>
        public void SaveData(ref GameData _data)
        {
            _data.gold = player.GetComponent<PlayerStats>().Gold;
            _data.strength = player.GetComponent<PlayerStats>().strength.GetValue();
            _data.agility = player.GetComponent<PlayerStats>().agility.GetValue();
            _data.intelligence = player.GetComponent<PlayerStats>().intelligence.GetValue();
            _data.vitality = player.GetComponent<PlayerStats>().vitality.GetValue();
        }
        /// <summary>
        /// Set data into player after loading
        /// </summary>
        /// <param name="data"></param>
        public void SetData(GameData data)
        {
            var stats = player.GetComponent<PlayerStats>();
            stats.Gold = data.gold;
            stats.strength.SetDefaultValue(data.strength);
            stats.agility.SetDefaultValue(data.agility);
            stats.intelligence.SetDefaultValue(data.intelligence);
            stats.vitality.SetDefaultValue(data.vitality);
        }
    }
}