using System.Collections.Generic;

namespace Save_and_Load
{
    [System.Serializable]
    public class GameData
    {
        public string screenName;
        public int gold;
        public int strength;
        public int agility;
        public int intelligence;
        public int vitality;
        public SerializableDictionary<string, int> inventory;
        public List<string> equipmentId;
        public string closeCheckpointId;

        public SerializableDictionary<string, bool> checkpoints;
        public GameData()
        {
            screenName = string.Empty;
            this.gold = 0;
            this.inventory = new SerializableDictionary<string, int>();
            this.equipmentId = new List<string>();
            this.closeCheckpointId = string.Empty;
            checkpoints = new SerializableDictionary<string, bool>();
        }
    }
}
