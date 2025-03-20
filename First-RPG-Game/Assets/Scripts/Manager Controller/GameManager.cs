using Save_and_Load;
using UnityEngine;

namespace Manager_Controller
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public GameData latestCheckpointData;
        public bool isDied = false;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void UpdateCheckpointData(GameData checkpointData)
        {
            latestCheckpointData = checkpointData;
        }
    }
}
