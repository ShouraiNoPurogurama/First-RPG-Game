using UnityEngine;

namespace MainCharacter
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        public Player player;

        private void Awake()
        {
            //When have multiple scenes
            if (Instance is not null)
            {
                Destroy(Instance.gameObject);
            }
            else
            {
                Instance = this;
            }
        }
    }
}