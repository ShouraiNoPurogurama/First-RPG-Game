using System.Collections.Generic;

namespace Save_and_Load
{
    [System.Serializable]
    public class TargetData
    {
        public string screenName;
        public int gold;
        public int strength;
        public int agility;
        public int intelligence;
        public int vitality;
        public Dictionary<string, int> inventory;
        public List<string> equipmentId;
        public string closeCheckpointId;
        public Dictionary<string, bool> checkPoints;
    }
}
