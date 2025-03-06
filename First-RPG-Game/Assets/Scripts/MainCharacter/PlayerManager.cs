using UnityEngine;

namespace MainCharacter
{
    public class PlayerManager : MonoBehaviour, ISaveManager
    {
        public static PlayerManager Instance;
        public Player player;
        public int currency;

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
            this.currency = _data.currency;
        }

        public void SaveData(ref GameData _data)
        {
            _data.currency = this.currency;
        }
    }
}